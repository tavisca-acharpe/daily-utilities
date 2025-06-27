using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using System.Text.Json;

namespace AWS_STS_Token_Generator
{
    public class AmazonSecurityTokenGenerator
    {
        public static AmazonSecurityTokenServiceClient AwsTokenGenerator()
        {
            string roleArn = "arn:aws:iam::346319152574:role/qa-taas-sts-role";
            string sessionName = "TemporarySessionKeys ";
            var stsClient = new AmazonSecurityTokenServiceClient();

            try
            {
                var assumeRoleRequest = new AssumeRoleRequest
                {
                    RoleArn = roleArn,
                    RoleSessionName = sessionName,
                    DurationSeconds = 14400
                };

                var response = stsClient.AssumeRoleAsync(assumeRoleRequest).Result;
                var credentials = response.Credentials;

                Console.WriteLine("Access Key: " + credentials.AccessKeyId);
                Console.WriteLine("Secret Key: " + credentials.SecretAccessKey);
                Console.WriteLine("Session Token: " + credentials.SessionToken);
                Console.WriteLine("Expiration: " + credentials.Expiration);

                // Create an object to store credentials data
                var credsToSave = new
                {
                    AccessKeyId = credentials.AccessKeyId,
                    SecretAccessKey = credentials.SecretAccessKey,
                    SessionToken = credentials.SessionToken,
                    Expiration = credentials.Expiration.ToString()
                };

                // Specify the path where you want to save the credentials JSON
                string savePath = @"C:\path\to\your\folder\aws-sts-credentials.json";

                // Serialize the credentials object to JSON
                string json = JsonSerializer.Serialize(credsToSave, new JsonSerializerOptions { WriteIndented = true });

                // Write the JSON string to the specified file
                File.WriteAllText(savePath, json);

                Console.WriteLine($"Credentials saved to {savePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error assuming role: " + ex.Message);
            }

            return stsClient;
        }
    }
}
