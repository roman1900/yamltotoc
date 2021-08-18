using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace yamltotoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();
            List<Document> documents = new List<Document>();
            var mdfiles = Directory.GetFiles(".","*.md");
            foreach (string doc in mdfiles)
            {
                if (doc != ".\readme.md") { //we will output the readme.md
                    var content = File.ReadAllLines(doc);
                    string yaml = "";
                    if (content[0].Replace(" ","")  == "---") { //yaml content in doc
                        for(int i = 1; i < content.Length && content[i].Replace(" ","") != "---"; i++)
                        {
                            yaml += content[i] + System.Environment.NewLine; 
                        }
                        Document currentdoc = deserializer.Deserialize<Document>(yaml);
                        currentdoc.docpath = doc.Replace(" ","%20").Replace(".\\","");
                        documents.Add(currentdoc);
                        yaml = "";
                    }
                }
            }

            if (documents.Count > 0) {
                string toc = "";
                toc +="|Title|Desription|"
                    + System.Environment.NewLine
                    + "|---|---|"
                    + System.Environment.NewLine;
                foreach (Document d in documents)
                {
                    toc += $"|[{d.title}]({d.docpath})|{d.shortdescription}|"
                            + System.Environment.NewLine;
                }
                File.WriteAllText("readme.md",toc);
            }
        }   
            

    }
}
