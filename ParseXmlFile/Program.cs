using System;
using System.Linq;
using System.Xml.Linq;

namespace xmltest
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileName = @"C:\SCAN\order.txt";
            var orderXml = XDocument.Load(fileName);
            var orderXmlString = orderXml.ToString();
            ProcessOrderXml(orderXmlString);
            Console.ReadLine();
        }

        public static void ProcessOrderXml(string orderXml)
        {
            //parse xml to get values
            var xmlDoc = XDocument.Parse(orderXml);

            var orderId = xmlDoc.Descendants("OrderId").FirstOrDefault()?.Value;
            Console.WriteLine($@"OrderId: {orderId}");
            var createdOnText = xmlDoc.Descendants("CreatedOn").FirstOrDefault()?.Value;
            Console.WriteLine($@"CreatedOnText: {createdOnText}");
            var createdBy = xmlDoc.Descendants("CreatedBy").FirstOrDefault()?.Value;
            Console.WriteLine($@"CreatedBy: {createdBy}");

            var quotationCode = xmlDoc.Descendants("Quotation").FirstOrDefault()?.Value;
            Console.WriteLine($@"QuotationCode: {quotationCode}");
            var clientCode = xmlDoc.Descendants("Client").FirstOrDefault()?.Value;
            Console.WriteLine($@"ClientCode: {clientCode}");

            var xmlFileName = $"EuroportalOrder{orderId}.xml";
            Console.WriteLine($@"XMLFileName: {xmlFileName}");

            var poNumber = xmlDoc.Descendants("PONumber").FirstOrDefault()?.Value;
            Console.WriteLine($@"PONumber: {poNumber}");

            var samples = xmlDoc.Descendants("Sample");

            //samples
            foreach (var sample in samples)
            {
                Console.WriteLine($"---------------------\n---Start of Sample---\n--------------------- ");
                var europortalId = sample.Descendants("SampleId").FirstOrDefault()?.Value;
                var europortalBarcode = sample.Descendants("Barcode").FirstOrDefault()?.Value;
                var description = sample.Descendants("Description").FirstOrDefault()?.Value;

                Console.WriteLine($@"europortalId: {europortalId}");
                Console.WriteLine($@"europortalBarcode: {europortalBarcode}");
                Console.WriteLine($@"description: {description}");

                //add sample additional fields
                var sampleAdditionalFields = sample.Descendants("AdditionalField");
                foreach (var additionalField in sampleAdditionalFields)
                {
                    var key = additionalField.Descendants("Key").FirstOrDefault()?.Value;
                    var value = additionalField.Descendants("Value").FirstOrDefault()?.Value;
                    Console.WriteLine($@"Key: {key}  - Value: {value}");
                }

                //add planning
                var quotationItems = sample.Descendants("QuotationItem");
                foreach (var quotationItem in quotationItems)
                {
                    var subGroupCode = quotationItem.Descendants("SubgroupCode").FirstOrDefault()?.Value;
                    var subGroupKey = quotationItem.Descendants("SubgroupKey").FirstOrDefault()?.Value;
                    var customerPackage = quotationItem.Descendants("CustomerPackage").FirstOrDefault()?.Value;

                    var salesParts = quotationItem.Descendants("SalesPart");
                    var hasSalesParts = quotationItem.Descendants("SalesPart").Any();
                    if (!hasSalesParts)
                    {
                        Console.WriteLine($@"No sales parts");
                        Console.WriteLine($@"subGroupCode: {subGroupCode}  - subGroupKey: {subGroupKey}");
                    }
                    foreach (var salesPart in salesParts)
                    {
                        var name = salesPart.Descendants("Name").FirstOrDefault()?.Value;
                        var type = salesPart.Descendants("Type").FirstOrDefault()?.Value;
                        var quoteLineKey = salesPart.Descendants("QuoteLineKey").FirstOrDefault()?.Value;
                        Console.WriteLine($@"subGroupCode: {subGroupCode}  - subGroupKey: {subGroupKey} - name {name} - type {type}");
                        Console.WriteLine($@"quoteLineKey: {quoteLineKey}");
                    }
                }

                var expectedValues = sample.Descendants("ExpectedValue");
                foreach (var expected in expectedValues)
                {
                    var testCode = expected.Attribute("TestCode2")?.Value;
                    var parameterCode = expected.Attribute("ParameterCode")?.Value;
                    var numeratorCode = expected.Attribute("Numerator")?.Value;
                    var denominatorCode = expected.Attribute("Denominator")?.Value;
                    var stringValue = expected?.Value;
                    Console.WriteLine($@"ExpectedValue: {stringValue}  - testCode: {testCode} - parameterCode {parameterCode} - numeratorCode {numeratorCode} - denominatorCode {denominatorCode}");
                }
            }
        }
    }
}
