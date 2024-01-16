﻿using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using NotVisualBasic.FileIO;
using System.Collections;
using System.Globalization;
using System.Text.Json.Nodes;
using TestTask.Models;

namespace TestTask.FileReader
{
    public class CsvFileParser
    {
      
        public IEnumerable  Read (Stream fileStream)
        {

            var csvParser = new CsvTextFieldParser(fileStream);
            while (!csvParser.EndOfData)
            {   
                string[]? fields = csvParser.ReadFields();
                if (fields is null)
                {
                    continue;
                }
                yield return fields;
            }
        }


        

    }


       


    
}
