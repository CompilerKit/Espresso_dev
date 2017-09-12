//Apache2, 2017, EngineKit 
using System;
namespace Espresso.TypeScript
{
    [JsType]
    public class EspressoHostForTsc_25
    {
        //function getEspressoSystem()
        //{
        //    my_expr_ext.ConsoleLog("test");
        //    var realpath = my_expr_ext.realpath && (function(path) { return my_expr_ext.realpath(path); });
        //    return {
        //        newLine: my_expr_ext.newLine || "\r\n",
        //        args: my_expr_ext.GetArgs(),
        //        useCaseSensitiveFileNames: !!my_expr_ext.useCaseSensitiveFileNames,
        //        write: my_expr_ext.ConsoleLog,
        //        readFile: function(path, _encoding) {
        //            return my_expr_ext.readFile(path);
        //        },
        //        writeFile: function(path, data, writeByteOrderMark) {
        //            if (writeByteOrderMark)
        //            {
        //                data = "\uFEFF" + data;
        //            }
        //            my_expr_ext.writeFile(path, data);
        //        },
        //    resolvePath: my_expr_ext.resolvePath,
        //    fileExists: my_expr_ext.fileExists,
        //    directoryExists: my_expr_ext.folderExists,
        //    createDirectory: my_expr_ext.createFolder,
        //    getExecutingFilePath: function() { return my_expr_ext.executingFile; },
        //    getCurrentDirectory: function() { return my_expr_ext.GetCurrentDir(); },
        //    getDirectories: my_expr_ext.getDirectories,
        //     getEnvironmentVariable: function(vname){
        //		return my_expr_ext.getEnvironmentVariable(vname);
        //	},
        //    readDirectory: function(path, extensions, excludes, includes, _depth) {
        //            var pattern = ts.getFileMatcherPatterns(path, excludes, includes, !!my_expr_ext.useCaseSensitiveFileNames, my_expr_ext.GetCurrentDir());
        //            return my_expr_ext.readDirectory(path, extensions, pattern.basePaths, pattern.excludePattern, pattern.includeFilePattern, pattern.includeDirectoryPattern);
        //        },
        //    exit: my_expr_ext.quit,
        //    realpath: realpath
        //    };
        //}
        [JsMethod]
        public string[] readDirectory(string path, string extensions, string basePaths, string excludePattern, string includeFilePattern, string includeDirectoryPattern)
        {
            //TODO: impl here
            return new string[0];
        }
        [JsMethod]
        public string getEnvironmentVariable(string varname)
        {
            return "";//test
        }
        [JsMethod]
        public string realPath(string relativePath)
        {
            return relativePath;
        }
        [JsMethod]
        public string resolvePath(string relativePath)
        {
            return relativePath;
        }
        [JsProperty]
        public string newLine
        {
            get
            {
                return "\r\n";
            }
        }
        [JsProperty]
        public bool useCaseSensitiveFileNames
        {
            get
            {
                return true;
            }
        }
        [JsProperty]
        public string executingFile
        {
            get
            {
                return "";
            }
        }
        [JsMethod]
        public string readFile(string filename)
        {
            if (filename == "greeter.ts")
            {

                string sampleFileContent2 =
                    @"
                        class Student {
                            fullName: string;
                            constructor(public firstName, public middleInitial, public lastName) {
                                this.fullName = firstName + "" "" + middleInitial + "" "" + lastName;
                            }
                         } 
                        interface Person
                        {
                            firstName: string;
                            lastName: string;
                        } 
                        function greeter(person : Person)
                        {
                            return ""Hello, "" + person.firstName + "" "" + person.lastName;
                        }
                        var user = new Student(""Jane"", ""M."",""User"");
                        document.body.innerHTML = greeter(user);
                    ";
                return sampleFileContent2;
            }
            else
            {
                //in this version we from sample folder only
                return System.IO.File.ReadAllText(filename);

                //string onlyFilename = System.IO.Path.GetFileName(filename);
                //if(onlyFilename =="index.ts")
                //if (System.IO.File.Exists("../Test3/TS/ts_samples/" + onlyFilename))
                //{
                //    return System.IO.File.ReadAllText("../Test3/TS/ts_samples/" + onlyFilename);
                //} 
                //return "";
            }
        }
        [JsMethod]
        public void writeFile(string filename, string data)
        {
            Console.WriteLine("req: write " + filename);
            Console.WriteLine("");
            Console.WriteLine("=== compiler output===");
            Console.WriteLine("");
            Console.WriteLine(data);
            Console.WriteLine("");
            Console.WriteLine("======");
            Console.WriteLine("");
        }
        [JsMethod]
        public string getDirectories(string path)
        {
            //get directory
            return "";
        }
        [JsMethod]
        public string GetAccessibleFileSystemEntries(string path)
        {
            //return { files: files, directories: directories };
            //        return JSON.parse(my_expr_ext.GetAccessibleFileSystemEntries(path));
            //get directory

            return "";
        }
        [JsMethod]
        public string GetCurrentDir()
        {
            return ".";
        }
        [JsMethod]
        public bool fileExists(string filename)
        {
            return false;
        }
        [JsMethod]
        public bool directoryExists(string folderName)
        {
            return false;
        }
        [JsMethod]
        public string GetScriptFullName()
        {
            return "hello1.ts";
        }
        [JsMethod]
        public void createFolder(string directoryName)
        {

        }
        [JsMethod]
        public void quit(int exitCode)
        {
            Console.WriteLine("quit:" + exitCode);
        }


        [JsMethod]
        public string GetArgs()
        {
            //return json string
            return "[]";
        }


        [JsMethod]
        public void ConsoleWrite(string msg)
        {
            Console.WriteLine("console write :" + msg);
        }
        [JsMethod]
        public void ConsoleLog(object o)
        {
            Console.WriteLine("console write :" + o.ToString());
        }
        [JsMethod]
        public object Require(string module)
        {
            //for require()
            if (module == "fs")
            {
                return this;
            }
            else
            {
                return null;
            }
        }

    }


}