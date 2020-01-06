using System;
using System.Collections.Generic;
using System.IO;

namespace VMTranslator
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName;
            string workingPath = Directory.GetCurrentDirectory();

            string fullVMPath;
            string fullASMPath;
            
            StreamReader streamReader;
            StreamWriter streamWriter;
            if(args.Length == 1)
            {
                fileName = args[0];
                fullVMPath = workingPath + "\\" + fileName + ".vm";
                fullASMPath = workingPath + "\\" + fileName + ".asm";
                Console.WriteLine(fullVMPath);
                Console.WriteLine(fullASMPath);
                if (File.Exists(fullVMPath))
                {
                    streamReader = File.OpenText(fullVMPath);
                }
                else
                {
                    Console.WriteLine("Could not find file: " + fileName);
                    throw new FileNotFoundException();
                    return;
                }

                if (File.Exists(fullASMPath))
                {
                    File.Delete(fullASMPath);
                    streamWriter = File.CreateText(fullASMPath);
                }
                else
                {
                    streamWriter = File.CreateText(fullASMPath);
                }
            }
            else
            {
                return;
            }
            
            Parser VMParser = new Parser();
            HackWriter ASMWriter = new HackWriter();

            string ToFile = new string(string.Empty);
            List<Token> curTokens = new List<Token>();

            string str;
            int i = 1;
            while((str = streamReader.ReadLine()) != null)
            {
                if(str.Length <= 2 || str.Trim() == string.Empty ||str.Substring(0,2) == "//")
                {
                    i++;
                    continue;
                }

                try
                {
                    VMParser.ParseString(str, out curTokens);
                }catch(Exception e)
                {
                    Console.WriteLine("ERROR on parsing line : " + i + " Message :" + e.Message);
                    return;
                }
                
                foreach(Token tkn in curTokens)
                {
                    Console.Write(tkn.ToString() + " ");
                }
                Console.WriteLine();

                try
                {
                    ASMWriter.TokensToHack(ref curTokens, out str);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR on writing line : " + i + " Message :" + e.Message);
                    return;
                }

                try
                {
                    streamWriter.WriteLine(str);
                }catch (Exception e)
                {
                    Console.WriteLine("ERROR on writing assembly to file" + i + " " + e.Message);
                    return;
                }

                i++;
            }

            //streamWriter.WriteLine("(FOO.END)");
            //streamWriter.WriteLine("@FOO.END");
            //streamWriter.WriteLine("0;JMP");

            streamWriter.Close();
            streamReader.Close();
        } 
    }
}
