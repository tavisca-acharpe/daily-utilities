using System.Net.Http.Headers;
using System.Text;

namespace AWS_STS_Token_Generator
{
    public class JenkinsTokenGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly string _jenkinsUrl;
        private readonly string _jobName;
        private readonly string _username;
        private readonly string _apiToken;

        public JenkinsTokenGenerator(string jenkinsUrl, string jobName, string username, string apiToken)
        {
            _jenkinsUrl = jenkinsUrl.TrimEnd('/');
            _jobName = jobName;
            _username = username;
            _apiToken = apiToken;

            _httpClient = new HttpClient();

            // Basic Auth Header
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_username}:{_apiToken}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        public static async Task JenkinsAwsTokenGenerator()
        {
            string jenkinsUrl = "https://jenkins.common.cnxloyalty.com/job/App-NGOS/job/DataMigration/job/";
            string jobName = "AWS-STS-Token";
            string username = "name";
            string apiToken = "token";

            var client = new JenkinsTokenGenerator(jenkinsUrl, jobName, username, apiToken);
            await client.TriggerJobAndReadConsoleAsync();
        }

        public async Task TriggerJobAndReadConsoleAsync()
        {
            // 1. Trigger the Jenkins Job
            var buildUrl = $"https://jenkins.common.cnxloyalty.com/job/App-NGOS/job/DataMigration/job/AWS-STS-Token";
            var buildResponse = await _httpClient.PostAsync(buildUrl, null);

            if (!buildResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to trigger job: {buildResponse.StatusCode}");
                return;
            }

            Console.WriteLine("Job triggered successfully. Waiting for build to start...");

            // 2. Get queue location from "Location" header
            if (!buildResponse.Headers.Contains("Location"))
            {
                Console.WriteLine("No queue location header found.");
                return;
            }
            var queueUrl = buildResponse.Headers.Location.ToString() + "api/json";

            // 3. Poll queue item until executable (build) starts and get build number
            int buildNumber = 0;
            while (buildNumber == 0)
            {
                var queueResponse = await _httpClient.GetAsync(queueUrl);
                var queueJson = await queueResponse.Content.ReadAsStringAsync();

                dynamic queueData = System.Text.Json.JsonSerializer.Deserialize<dynamic>(queueJson);

                if (queueData.executable != null)
                {
                    buildNumber = (int)queueData.executable.number;
                    Console.WriteLine($"Build started. Build number: {buildNumber}");
                    break;
                }

                Console.WriteLine("Waiting for build to start...");
                await Task.Delay(3000);
            }

            // 4. Poll the build console output until build completes
            string consoleOutputUrl = $"{_jenkinsUrl}/job/{_jobName}/{buildNumber}/logText/progressiveText";

            int start = 0;
            bool moreData = true;

            Console.WriteLine("Fetching console output:");

            while (moreData)
            {
                var response = await _httpClient.GetAsync($"{consoleOutputUrl}?start={start}");
                var text = await response.Content.ReadAsStringAsync();

                Console.Write(text);

                start += text.Length;

                if (response.Headers.Contains("X-More-Data"))
                {
                    moreData = bool.Parse(response.Headers.GetValues("X-More-Data").First());
                }
                else
                {
                    moreData = false;
                }

                await Task.Delay(2000); // wait 2 seconds before fetching more
            }

            Console.WriteLine("\nBuild complete.");
        }
    }
}
