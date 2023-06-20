using System;
using System.Reflection;
using System.Xml.Linq;
using System.IO;

namespace sd6lab
{
	public class Program
	{

		static string GetPath(string name)
		{

			string[] start = Assembly.GetExecutingAssembly().Location.Split('\\');
			string path = "";
			for (int i = 0; i < 6; i++)
			{
				path += start[i];
				path += "\\";
			}
			path += $"Resources\\{name}";
			return path;
		}

		private static bool Task2(string inputFileName, string outputFileName)
		{

			string[] lines = File.ReadAllLines(inputFileName);

			XElement rootElement = new XElement("root", lines.Select((line, index) => new XElement("line" + (index + 1), line)));
			XDocument xmlDoc = new XDocument(rootElement);

			xmlDoc.Save(outputFileName);
			return true;
		}
		
		private static bool Task12(string inputFileName, string outputFileName)
		{
			XDocument xmlLoadDoc = XDocument.Load(inputFileName);

			var elementCounts = xmlLoadDoc.Root.Elements()
			.GroupBy(e => e.Attribute("name").Value)
			.Select(g => new { Name = g.Key, Count = g.Count() });

			XDocument outputXmlDoc = new XDocument(new XElement("root", elementCounts.Select(e => new XElement("line", $"Имя: {e.Name}, количество: {e.Count}"))));

			outputXmlDoc.Save(outputFileName);

			return true;
		}

		private static bool Task22(string inputFileName, string outputFileName,string stringToRemove)
		{
			XDocument xmlDoc = XDocument.Load(inputFileName);
			var temp = xmlDoc.Descendants();
			xmlDoc.Descendants().Where(e => e.Name == stringToRemove && e.Parent != xmlDoc.Root).Remove();
			xmlDoc.Save(outputFileName);
			return true;
		}
		
		private static bool Task32(string inputFileName, string outputFileName, string S1, string S2)
		{
			XDocument xmlDoc = XDocument.Load(inputFileName);
			var targetElement = xmlDoc.Descendants(S1).FirstOrDefault();

			if (targetElement != null)
			{
				// Создать элемент S2
				XElement newElement = new XElement(S2);

				// Проверить наличие дочерних элементов у элемента S1
				if (targetElement.Elements().Any())
				{
					// Получить последний атрибут и первый дочерний элемент следующего элемента
					XAttribute lastAttribute = targetElement.Attributes().LastOrDefault();
					XElement firstChild = targetElement.Elements().FirstOrDefault();

					if (lastAttribute != null)
					{
						// Добавить последний атрибут в элемент S2
						newElement.Add(new XAttribute(lastAttribute.Name, lastAttribute.Value));
					}

					if (firstChild != null)
					{
						// Добавить первый дочерний элемент следующего элемента в элемент S2
						newElement.Add(firstChild);
					}
				}
				else
				{
					// Добавить комбинированный тег в элемент S2
					newElement.Value = targetElement.Value;
				}

				// Вставить элемент S2 перед элементом S1
				targetElement.AddBeforeSelf(newElement);
			}
			xmlDoc.Save(outputFileName);
			return true;
		}
		


		private static bool Task42(string inputFileName, string outputFileName)
		{
			XDocument xmlDoc = XDocument.Load(inputFileName);

			var elements = xmlDoc.Root.Elements()/*.Where(e => e.Elements().Any())*/;

			foreach (var element in elements)
			{
				double sum = element.Elements().SelectMany(e => e.Attributes()).Sum(num => double.Parse(num.Value));
				string sumValue = sum.ToString("0.##");
				element.Add(new XElement("sum", sumValue));
			}
			xmlDoc.Save(outputFileName);
			return true;
		}
		
		private static bool Task52(string inputFileName, string outputFileName)
		{
			XDocument xmlDoc = XDocument.Load(inputFileName);

			foreach (var element in xmlDoc.Elements().Elements())
			{
				int year = DataCheck(element, "year", 2000);
				int month = DataCheck(element, "month", 1);
				int day = DataCheck(element, "day", 10);

				XElement dateElement = new XElement("date", new DateTime(year, month, day).ToString("yyyy-MM-dd"));

				element.AddFirst(dateElement);
				element.Attribute("year")?.Remove();
				element.Attribute("month")?.Remove();
				element.Attribute("day")?.Remove();
			}
			xmlDoc.Save(outputFileName);
			return true;
		}

		static int DataCheck(XElement element, string attributeName, int defaultValue)
		{
			XAttribute attribute = element.Attribute(attributeName);
			if (attribute != null && int.TryParse(attribute.Value, out int value))
			{
				return value;
			}
			return defaultValue;
		}

		private static bool Task62(string inputFileName, string outputFileName)
		{
			XDocument xmlDoc = XDocument.Load(inputFileName);

			var transformedElements = xmlDoc.Root
			.Elements()
			.OrderBy(element => GetClientCode(element))
			.Select(element => new XElement("id" + GetClientCode(element),
				new XAttribute("date", GetDateAttributeValue(element.Element("date"))),
				GetMinutesAttributeValue(element.Element("time")).ToString()));

			xmlDoc.Root.ReplaceAll(transformedElements);


			xmlDoc.Save(outputFileName);
			return true;
		}

		static int GetClientCode(XElement element)
		{
			int code = 0;
			XElement idElement = element.Element("id");
			if (idElement != null && int.TryParse(idElement.Value, out int parsedCode))
			{
				code = parsedCode;
			}
			return code;
		}

		static string GetDateAttributeValue(XElement element)
		{
			if (element != null && DateTime.TryParse(element.Value, out DateTime date))
			{
				return date.ToString("yyyy-MM-ddTHH:mm:ss");
			}
			return "";
		}

		static int GetMinutesAttributeValue(XElement element)
		{
			if (element != null && element.Value.StartsWith("PT") && element.Value.EndsWith("M"))
			{
				int minutes = 0, hour = 0;
				string durationHour = element.Value.Substring(2, 1);
				string durationMinutes = element.Value.Substring(4, 2);
				int.TryParse(durationHour, out hour);
				int.TryParse(durationMinutes, out minutes);
				return hour * 60 + minutes;
				
			}
			return 0;
		}


		public static void Main(string[] args)
		{
			Task2(GetPath("2task.txt"), GetPath("2task.xml"));
			Task12(GetPath("12taskInput.xml"), GetPath("12taskOutput.xml"));
			Task22(GetPath("22taskInput.xml"), GetPath("22taskOutput.xml"), "word");
			Task32(GetPath("32taskInput.xml"), GetPath("32taskOutput.xml"), "element1", "newElement");
			Task42(GetPath("42taskInput.xml"), GetPath("42taskOutput.xml"));
			Task52(GetPath("52taskInput.xml"), GetPath("52taskOutput.xml"));
			Task62(GetPath("62taskInput.xml"), GetPath("62taskOutput.xml"));
		}
	}
}