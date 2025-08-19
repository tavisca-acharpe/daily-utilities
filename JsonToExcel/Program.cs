using System;
using System.Data;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;

class Program
{
    static void Main(string[] args)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        string jsonPath = "source.json";
        string jsonString = File.ReadAllText(jsonPath);

        var root = JObject.Parse(jsonString);

        var schemas = root["schemaChanges"]?["addSchema"] as JArray;
        var relations = root["relationChanges"]?["addRelation"] as JArray;

        var schemaTable = new DataTable("Schemas");
        schemaTable.Columns.AddRange(new[]
        {
            new DataColumn("name"),
            new DataColumn("description"),
            new DataColumn("isSystemType"),
            new DataColumn("isBusinessCritical"),
            new DataColumn("isPIIData"),
            new DataColumn("isEncrypted"),
            new DataColumn("doNotIndex")
        });

        var propertiesTable = new DataTable("Properties");
        propertiesTable.Columns.AddRange(new[]
        {
            new DataColumn("schemaName"),
            new DataColumn("name"),
            new DataColumn("valueType"),
            new DataColumn("description"),
            new DataColumn("isMandatory"),
            new DataColumn("isMultiValue"),
            new DataColumn("isSystemType"),
            new DataColumn("doNotIndex"),
            new DataColumn("isDeleted"),
            new DataColumn("isEncrypted"),
            new DataColumn("isPIIData"),
            new DataColumn("isBusinessCritical"),
            new DataColumn("category")
        });

        var relationTable = new DataTable("Relations");
        relationTable.Columns.AddRange(new[]
        {
            new DataColumn("name"),
            new DataColumn("description"),
            new DataColumn("isSystemType"),
            new DataColumn("in_name"),
            new DataColumn("in_multiplicity"),
            new DataColumn("in_alias"),
            new DataColumn("out_name"),
            new DataColumn("out_multiplicity"),
            new DataColumn("out_alias")
        });

        // Process schemas and properties
        if (schemas != null)
        {
            foreach (var schema in schemas)
            {
                schemaTable.Rows.Add(
                    schema["name"],
                    schema["description"],
                    schema["isSystemType"],
                    schema["isBusinessCritical"],
                    schema["isPIIData"],
                    schema["isEncrypted"],
                    schema["doNotIndex"]
                );

                var properties = schema["properties"] as JArray;
                if (properties != null)
                {
                    foreach (var prop in properties)
                    {
                        propertiesTable.Rows.Add(
                            schema["name"], // schemaName
                            prop["name"],
                            prop["valueType"],
                            prop["description"],
                            prop["isMandatory"],
                            prop["isMultiValue"],
                            prop["isSystemType"],
                            prop["doNotIndex"],
                            prop["isDeleted"],
                            prop["isEncrypted"],
                            prop["isPIIData"],
                            prop["isBusinessCritical"],
                            prop["category"]
                        );
                    }
                }
            }
        }

        // Process relations
        if (relations != null)
        {
            foreach (var rel in relations)
            {
                relationTable.Rows.Add(
                    rel["name"],
                    rel["description"],
                    rel["isSystemType"],
                    rel["in"]?["name"],
                    rel["in"]?["multiplicity"],
                    rel["in"]?["alias"],
                    rel["out"]?["name"],
                    rel["out"]?["multiplicity"],
                    rel["out"]?["alias"]
                );
            }
        }

        // Insert empty row after each group of properties with the same schemaName
        DataTable updatedPropertiesTable = propertiesTable.Clone();

        var groupedRows = propertiesTable.AsEnumerable()
            .GroupBy(r => r.Field<string>("schemaName"));

        foreach (var group in groupedRows)
        {
            foreach (var row in group)
            {
                updatedPropertiesTable.ImportRow(row);
            }
            // Add an empty row after each schemaName group
            updatedPropertiesTable.Rows.Add(updatedPropertiesTable.NewRow());
        }

        // Save to Excel
        try
        {
            using (var package = new ExcelPackage())
            {
                package.Workbook.Worksheets.Add("Schemas").Cells["A1"].LoadFromDataTable(schemaTable, true);
                package.Workbook.Worksheets.Add("Properties").Cells["A1"].LoadFromDataTable(updatedPropertiesTable, true);
                package.Workbook.Worksheets.Add("Relations").Cells["A1"].LoadFromDataTable(relationTable, true);

                string outputPath = @"C:\Users\I759407\source\daily-utilities\JsonToExcel\output.xlsx";
                package.SaveAs(new FileInfo(outputPath));
                Console.WriteLine($"Excel file created at: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving Excel file: " + ex.Message);
        }
    }
}
