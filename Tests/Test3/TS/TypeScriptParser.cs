//Apache2, 2017, EngineKit 
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Espresso.TypeScript
{

    public class TypeScriptParser : IDisposable
    {
        JsEngine engine;
        StringBuilder stbuilder;

        public TypeScriptParser()
        {
            //init typescript parser engine
            int version = JsBridge.LibVersion;
            //read typescript src code
            string tsc_esprCode = File.ReadAllText("../../Samples/js_tools/tsc2_5/tsc_espr.js");
            stbuilder = new StringBuilder();
            stbuilder.Append(tsc_esprCode);
            engine = new JsEngine();
        }
        public void Dispose()
        {
            if (engine != null)
            {
                engine.Dispose();
                engine = null;
            }
        }
        public void Parse(string filename)
        {
            using (JsContext ctx = engine.CreateContext(new MyJsTypeDefinitionBuilder()))
            { 
                var my_expr_ext = new EspressoHostForTsc_25();
                ctx.SetVariableAutoWrap("my_expr_ext", my_expr_ext);
                string testsrc = @"(function(filename){                          
                        my_expr_ext.ConsoleLog(filename);
                        //parse
                        const sourceFile = ts.createSourceFile(filename,
                        my_expr_ext.readFile(filename),2, false);
                        //send output as json to managed host
                
                        //see https://stackoverflow.com/questions/11616630/json-stringify-avoid-typeerror-converting-circular-structure-to-json/11616993
                        var cache = [];
                        //var json_result= JSON.stringify( sourceFile);
                        var json_result= JSON.stringify(sourceFile, function(key, value) {
                            if (typeof value === 'object' && value !== null) {
                                if (cache.indexOf(value) !== -1) {
                                    // Circular reference found, discard key
                                    return;
                                }
                                // Store value in our collection
                                cache.push(value);
                            }
                            return value;
                        });
                        cache = null; // Enable garbage collection
                        return json_result;
                    })(""" + filename + "\")";
                stbuilder.Append(testsrc);

                object json_result = ctx.Execute(stbuilder.ToString());

                TsFromJson tsFromJson = new TsFromJson();
                tsFromJson.Build(json_result as string);
            }
        }
    }



}