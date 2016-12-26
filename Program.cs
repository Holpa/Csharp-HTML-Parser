using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;


namespace ParserJsonSiteSecure
{
    [DataContract(Name = "RFID")]
    public class RFID
    {
        [DataMember(Name = "code")]
        public string code { get; set; }

        [DataMember(Name = "user")]
        public string user { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }

        [DataMember(Name = "address")]
        public string address { get; set; }

        [DataMember(Name = "permission")]
        public string permission { get; set; }

        [DataMember(Name = "port")]
        public string port { get; set; }

        [DataMember(Name = "timestamp")]
        public string timestamp { get; set; }

    }




    [DataContract(Name = "Inventory")]
    public class Inventory
    {
        [DataMember(Name = "tags")]
        public string[] tags;
        [DataMember(Name = "tag")]
        public string tag { get; set; }
    }

    [DataContract(Name = "TAG")]
    public class TAG
    {
        [DataMember(Name = "token")]
        public string token { get; set; }
        [DataMember(Name = "vehicle")]
        public string vehicle { get; set; }

        [DataMember(Name = "report")]
        public string report { get; set; }

        [DataMember(Name = "gps")]
        public string gps { get; set; }

        [DataMember(Name = "DOP")]
        public string DOP { get; set; }
        //INVENTORY WILL BE NESTED
        [DataMember(Name = "inventory")]
        public Inventory inventory { get; set; }
    }

    [DataContract(Name = "LoginResponse")]
    public class LoginResponse
    {
        [DataMember(Name = "code")]
        public string code { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }

        [DataMember(Name = "result")]
        public Result result { get; set; }
    }

    [DataContract(Name = "VehicleInventoryReportResponse")]
    public class VehicleInventoryReportResponse
    {
        [DataMember(Name = "code")]
        public string code { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }
    }
    [DataContract(Name = "LogoutResponse")]
    public class LogoutResponse
    {
        [DataMember(Name = "code")]
        public string code { get; set; }

        [DataMember(Name = "result")]
        public Result result { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }
    }

    [DataContract(Name = "result")]
    public class Result
    {
        [DataMember(Name = "address")]
        public string address { get; set; }
        [DataMember(Name = "permission")]
        public string permission { get; set; }
        [DataMember(Name = "port")]
        public string port { get; set; }
        [DataMember(Name = "timestamp")]
        public string timestamp { get; set; }
        [DataMember(Name = "token")]
        public string token { get; set; }
        [DataMember(Name = "user")]
        public string user { get; set; }

    }

    class Program
    {
        // prepration to deal with the HTML string
        public void ParserSiteSecureTags(string htmlString)
        {
            int ArrayCount = (Regex.Matches(htmlString, "item").Count) / 2;

            string shellJson = "{" +
      "\"vehicle\":\" \"," +
      "\"report\":\" \"," +
      "\"gps\":\" \"," +
      "\"DOP\":\" \"," +
      "\"inventory\":{" +
                "\"tags\":[\" \" ]}}";
            if (ArrayCount > 1)
            {
                for (int i = 0; i < ArrayCount - 1; i++)
                {
                    shellJson = shellJson.Substring(0, shellJson.Length - 4);
                    shellJson += ",\" \" ]}} ";
                }
            }


            JavaScriptSerializer ser = new JavaScriptSerializer();
            var jsonObject = ser.Deserialize<TAG>(shellJson);

            jsonObject = bestParser(htmlString, jsonObject, ArrayCount);
        }

        //method that will Parse the HTML string and fillup the JSON
        private TAG bestParser(string text, TAG JsonOj, int ArrayCount)
        {
            string value;

            int first;
            int last;
            int between;

            //vehicle
            first = text.IndexOf("vehicle") + "vehicle".Length + 16;
            last = text.LastIndexOf("vehicle") - 2;
            between = last - first;
            value = text.Substring(first, between);
            JsonOj.vehicle = value;
            //report
            first = text.IndexOf("report") + "report".Length + 16;
            last = text.LastIndexOf("report") - 2;
            between = last - first;
            value = text.Substring(first, between);
            JsonOj.report = value;

            //gps
            first = text.IndexOf("gps") + "gps".Length + 16;
            last = text.LastIndexOf("gps") - 2;
            between = last - first;
            value = text.Substring(first, between);
            JsonOj.gps = value;

            //DOP

            first = text.IndexOf("DOP") + "DOP".Length + 16;
            last = text.LastIndexOf("DOP") - 2;
            between = last - first;
            value = text.Substring(first, between);
            JsonOj.DOP = value;

            //Inventory

            first = text.IndexOf("inventory") + "inventory".Length + 16;
            last = text.LastIndexOf("inventory") - 3;
            between = last - first;
            value = text.Substring(first, between);
            text = value;

            for (int i = 0; i < ArrayCount; i++)
            {
                first = text.IndexOf("string") + "string".Length + 2;
                last = text.IndexOf("/item") - 1;
                between = last - first;
                value = text.Substring(first, between);
                JsonOj.inventory.tags.SetValue(value, i);
                text = text.Substring(last + "/item".Length + 2);
            }

            return JsonOj;
        }

        static void Main(string[] args)
        {

            Program p = new Program();
            p.ParserSiteSecureTags("<root type=\"object\">\r\n  < vehicle type =\"string\">0614</vehicle>\r\n    < report type =\"string\">ChefdfghfhfsfdsfskIn</report>\r\n      < gps type =\"string\">45.552928,-73.735628</gps>\r\n        < DOP type =\"string\">1.0</DOP>\r\n          < inventory type =\"object\">\r\n              < tags type =\"array\">\r\n                  < item type =\"string\">00001371 1</item>\r\n                    < item type =\"string\">00001369 1</item>\r\n                      < item type =\"string\">00001372 1</item>\r\n                        < item type =\"string\">00001365 1</item>\r\n                          < item type =\"string\">00000000 0 45.552928,-73.735678</item>\r\n                            < item type =\"string\">000012E6 1</item>\r\n                              < item type =\"string\">000012DB 1</item>\r\n                                < item type =\"string\">0000136F 1</item>\r\n                                  < item type =\"string\">00001234 1</item>\r\n                                  </ tags >\r\n </ inventory >\r\n </ root > ");

        }
    }

    //  public String text = "<root type=\"object\">\r\n  < vehicle type =\"string\">0614</vehicle>\r\n    < report type =\"string\">ChefdfghfhfsfdsfskIn</report>\r\n      < gps type =\"string\">45.552928,-73.735628</gps>\r\n        < DOP type =\"string\">1.0</DOP>\r\n          < inventory type =\"object\">\r\n              < tags type =\"array\">\r\n                  < item type =\"string\">00001371 1</item>\r\n                    < item type =\"string\">00001369 1</item>\r\n                      < item type =\"string\">00001372 1</item>\r\n                        < item type =\"string\">00001365 1</item>\r\n                          < item type =\"string\">00000000 0 45.552928,-73.735678</item>\r\n                            < item type =\"string\">000012E6 1</item>\r\n                              < item type =\"string\">000012DB 1</item>\r\n                                < item type =\"string\">0000136F 1</item>\r\n                                  < item type =\"string\">00001234 1</item>\r\n                                  </ tags >\r\n </ inventory >\r\n </ root > ";

    //  // test bed
    //  // end test M bed
    //  // ISSUE WITH + - sign on some of the tags RADING , GPS!!!! as well
    //  // remove duplicates
    //  public void ParserJson()
    //  {
    //      int ArrayCount = (Regex.Matches(text, "item").Count) / 2;

    //      string shellJson = "{" +
    //"\"vehicle\":\" \"," +
    //"\"report\":\" \"," +
    //"\"gps\":\" \"," +
    //"\"DOP\":\" \"," +
    //"\"inventory\":{" +
    //          "\"tags\":[\" \" ]}}";
    //      if(ArrayCount >1)
    //      {
    //          for(int i =0; i<ArrayCount-1; i++)
    //          {
    //              shellJson = shellJson.Substring(0, shellJson.Length - 4);
    //              shellJson += ",\" \" ]}} ";
    //          }
    //      }


    //      JavaScriptSerializer ser = new JavaScriptSerializer();
    //      //var ab = ser.Deserialize<RFID>(res);
    //      var jsonObject = ser.Deserialize<TAG>(shellJson);
    //      // just replace the JSON ids

    //      jsonObject = bestParser(text, jsonObject,ArrayCount);
    //      // removing special character + HTML
    //      text = Regex.Replace(text, @"[^0-9a-zA-Z]+", " "); // need to be WEAKER
    //      text = text.Replace("string", "");
    //      text = text.Replace("type", "");
    //      text = text.Replace("root", "");
    //      text = text.Replace("object", "");
    //      text = text.Replace("item", "");
    //      text = text.Replace("array", "");
    //      // at this point no more random characters ONLY redundency
    //      text = text.Insert(0, "{");
    //      text = text.Replace("vehicle", "\"vehicle\":");
    //      text = text.Replace("report", "\"report\":");
    //      text = text.Replace("gps", "\"gps\":");
    //      text = text.Replace("DOP", "\"DOP\":");
    //      text = text.Replace("inventory", "\"inventory\":{");
    //      text = text.Replace("tags", "\"tags\":[");
    //      text = text.Remove(text.Length - 25, 25);
    //      text += "]}}";
    //      // fix the Array, Need to know before hand how many members in the array.. 
    //      //try by counting how many times ITEMS have been repeated in the HTML <<< SUPER IDEA
    //      // and that will be ur array members
    //      text = Regex.Replace(text, @"\s+", "");
    //      if (text.Contains("tags"))
    //      {
    //          int tagsArray = text.IndexOf("tags");
    //          int spot = tagsArray + 7;
    //          int i = 1;
    //          while (i < ArrayCount)
    //          {
    //              try
    //              {
    //                  // first tag

    //                  text = text.Insert(spot, "\"");
    //                  text = text.Insert(9 + spot, " ");
    //                  text = text.Insert(11 + spot, "\"");
    //                  text = text.Insert(12 + spot, ",");
    //                  spot = spot + 13;
    //                  // other tags
    //                  i++;
    //                  if (i == 5)
    //                  {
    //                      text = text.Insert(spot, "\"");
    //                      text = text.Insert(9 + spot, " ");
    //                      text = text.Insert(11 + spot, " ");
    //                      text = text.Insert(20 + spot, ",");
    //                      text = text.Insert(30 + spot, "\"");
    //                      text = text.Insert(31 + spot, ",");
    //                      spot = spot + 31;
    //                  }
    //              }
    //              catch (Exception e)
    //              {

    //              }

    //          }// end while
    //          int lastIndex = text.LastIndexOf(',');
    //          text = text.Remove(lastIndex, 1);
    //      }


    //  }





    //  // USING LINQ powerful method! not used
    //  public Object WordCounter()
    //  {


    //      string[] source = text.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

    //      var wordsAndCount = source.GroupBy(x => x.ToLower()).Select(x => new { Word = x.Key, Count = x.Count() }).OrderByDescending(word => word.Count).ThenBy(word => word.Word);

    //      foreach (var wordWithCount in wordsAndCount)
    //      {
    //          Console.WriteLine(wordWithCount.Word + "\t\t" + wordWithCount.Count);
    //      }

    //      Console.WriteLine("Press any key to exit");
    //      Console.ReadKey();
    //      return wordsAndCount;
    //  }
    //  public string getString()
    //  {
    //      return "<root type=\"object\">\r\n  < vehicle type =\"string\">0614</vehicle>\r\n    < report type =\"string\">ChefdfghfhfsfdsfskIn</report>\r\n      < gps type =\"string\">45.552928,-73.735628</gps>\r\n        < DOP type =\"string\">1.0</DOP>\r\n          < inventory type =\"object\">\r\n              < tags type =\"array\">\r\n                  < item type =\"string\">00001371 1</item>\r\n                    < item type =\"string\">00001369 1</item>\r\n                      < item type =\"string\">00001372 1</item>\r\n                        < item type =\"string\">00001365 1</item>\r\n                          < item type =\"string\">00000000 0 45.552928,-73.735678</item>\r\n                            < item type =\"string\">000012E6 1</item>\r\n                              < item type =\"string\">000012DB 1</item>\r\n                                < item type =\"string\">0000136F 1</item>\r\n                                  < item type =\"string\">00001234 1</item>\r\n                                  </ tags >\r\n </ inventory >\r\n </ root > ";
    //  }
}
