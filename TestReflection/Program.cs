using System;
using System.Data;
using System.Reflection;

namespace TestReflexiya
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientAccessLog clientAccessLog = new ClientAccessLog();
            PropertyInfo[] properties = typeof(ClientAccessLog).GetProperties();

            var row = GetDR();
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(clientAccessLog, LeadObjects(property.PropertyType, row[property.Name]));
                //Console.WriteLine($"{property.Name} {property.PropertyType} {Type.GetTypeCode(property.PropertyType)}");

            }

            Console.WriteLine(clientAccessLog.ClientId);

            Console.WriteLine();

            foreach (var item in row.ItemArray)
            {
                Console.WriteLine($"{Convert.GetTypeCode(item)} {item}");
            }

            Console.ReadKey();
        }

        public static DataRow GetDR()
        {
            DataTable dt = new DataTable("ClientAccessCardLog");
            dt.Clear();
            dt.Columns.Add("ClientId", typeof(decimal));
            dt.Columns.Add("ClientName", typeof(string));
            dt.Columns.Add("Birthday", typeof(DateTime));
            dt.Columns.Add("Age", typeof(decimal));
            dt.Columns.Add("TypeDoc", typeof(decimal));

            DataRow row = dt.NewRow();
            row["ClientId"] = "99465";
            row["ClientName"] = "Иванов Иван Иванович";
            row["Birthday"] = new DateTime(1990, 5, 6);
            row["Age"] = 31;
            row["TypeDoc"] = 1;

            dt.Rows.Add(row);
            dt.WriteXmlSchema("schema.xml");
            dt.WriteXml("table.xml");

            return row;
        }

        //public static object LeadObjects(TypeCode thisTypeCode, object distObject)
        public static object LeadObjects(Type thisType, object distObject)
        {
            TypeCode thisTypeCode = Type.GetTypeCode(thisType);
            try
            {
                //TypeCode thisTypeCode = Type.GetTypeCode(thisType);
                return Convert.ChangeType(distObject, thisTypeCode);
            }
            catch (Exception e)
            {
                switch (thisTypeCode)
                {
                    case TypeCode.Int32: return default(int);
                    case TypeCode.DateTime: return default(DateTime);
                    case TypeCode.Decimal: return default(decimal);
                    default: return null;
                }
            }
        }
    }

    public class ClientAccessLog
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime Birthday { get; set; }
        public int Age { get; set; }
        public TypeDoc TypeDoc { get; set; }
    }

    public enum TypeDoc
    {
        Undefined = 0,
        CloverCard,
        Passport
    }
}