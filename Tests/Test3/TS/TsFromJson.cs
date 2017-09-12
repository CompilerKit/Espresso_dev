//Apache2, 2017, EngineKit 
using System;
using SharpConnect.Data;

namespace Espresso.TypeScript
{
    static class EsElemExtractHelper
    {
        public static EsElem Type(this EsElem elem)
        {
            return elem.GetAttributeValueAsElem("type");
        }
        public static EsArr Args(this EsElem elem)
        {
            return elem.GetAttributeValueAsArray("arguments");
        }

        public static EsElem Expr(this EsElem elem)
        {
            return elem.GetAttributeValueAsElem("expression");
        }
        public static SyntaxKind Kind(this EsElem node)
        {
            return (SyntaxKind)node.GetAttributeValueAsInt32("kind");
        }
        public static EsArr Members(this EsElem elem)
        {
            return elem.GetAttributeValueAsArray("members");
        }
    }


    public class TsFromJson
    {
        //TS2.6
        public void Build(string jsonstr)
        {
            //parse json data
            EsElem jsonObj = ParseJson(jsonstr);
            //get statements
            ReadModuleBody(jsonObj);
        }
        void ReadTypeAliasDecl(EsElem node)
        {

        }
        void ReadModuleBody(EsElem node)
        {
            EsArr stmts = node.GetAttributeValueAsArray("statements");
            if (stmts == null)
            {
                //?
                return;
            }
            int j = stmts.Count;
            for (int i = 0; i < j; ++i)
            {
                EsElem stmt = (EsElem)stmts[i];
                SyntaxKind stmt_kind = stmt.Kind();
                switch (stmt_kind)
                {
                    default:
                        throw new NotSupportedException();
                    case SyntaxKind.ClassDeclaration:
                        ReadClassDecl(stmt);
                        break;
                    case SyntaxKind.InterfaceDeclaration:
                        ReadInterfaceDecl(stmt);
                        break;
                    case SyntaxKind.FunctionDeclaration:
                        ReadFuncDecl(stmt);
                        break;
                    case SyntaxKind.VariableStatement:
                        ReadVariableStatement(stmt);
                        break;
                    case SyntaxKind.ExpressionStatement:
                        ReadExpressionStatement(stmt);
                        break;
                    case SyntaxKind.ModuleDeclaration:
                        ReadModuleDecl(stmt);
                        break;
                    case SyntaxKind.EnumDeclaration:
                        ReadEnumDecl(stmt);
                        break;
                    case SyntaxKind.IfStatement:
                        //? -->found in tsc.ts
                        break;
                    case SyntaxKind.ExportDeclaration:
                    case SyntaxKind.ExportAssignment:
                    case SyntaxKind.NamespaceExportDeclaration:
                    case SyntaxKind.ImportEqualsDeclaration:
                    case SyntaxKind.ImportDeclaration:
                    case SyntaxKind.TypeAliasDeclaration:
                        //TODO:
                        break;

                }
            }
        }
        void ReadModuleDecl(EsElem node)
        {
            ReadNameSyntax(node);
            EsElem body = node.GetAttributeValueAsElem("body");
            ReadModuleBody(body);
        }

        void ReadNameSyntax(EsElem node)
        {
            EsElem es_name = node.GetAttributeValueAsElem("name");
            if (es_name != null)
            {
                string name = es_name.GetAttributeValueAsString("escapedText");
            }
            
        }

        void ReadVariableStatement(EsElem node)
        {
            //variable decl stmt
            EsElem declList = node.GetAttributeValueAsElem("declarationList");
            EsArr declarations = declList.GetAttributeValueAsArray("declarations");

            int j = declarations.Count;
            for (int i = 0; i < j; ++i)
            {
                EsElem declaration = (EsElem)declarations[i];
                ReadNameSyntax(declaration);
                //read initializer if not null
                EsElem initiailizer = declaration.GetAttributeValueAsElem("initializer");
                if (initiailizer != null)
                {
                    ReadExpression(initiailizer);
                }
            }
        }
        void ReadExpression(EsElem node)
        {
            SyntaxKind expr_kind = node.Kind();
            switch (expr_kind)
            {
                default:
                    //this steps we skip other expressions
                    break;
                case SyntaxKind.TemplateExpression:
                    break;
                case SyntaxKind.Identifier:
                    break;
                case SyntaxKind.CallExpression:
                    ReadCallExpression(node);
                    break;
                case SyntaxKind.PropertyAccessExpression:
                    ReadPropertyAccessExpression(node);
                    break;
                case SyntaxKind.BinaryExpression:
                    ReadBinaryExpression(node);
                    break;
                case SyntaxKind.StringLiteral:
                    //return string literal 
                    break;
                case SyntaxKind.NewExpression:
                    ReadNewExpression(node);
                    break;
            }
        }
        void ReadCallExpression(EsElem node)
        {
            //call target
            ReadExpression(node.Expr());
            ReadArgumentList(node.Args());
        }

        void ReadPropertyAccessExpression(EsElem node)
        {
            //target expression
            ReadExpression(node.Expr());

            //prop name
            ReadNameSyntax(node);
        }
        void ReadBinaryExpression(EsElem node)
        {
            //left op right
            ReadExpression(node.GetAttributeValueAsElem("left"));
            EsElem op = node.GetAttributeValueAsElem("op");
            ReadExpression(node.GetAttributeValueAsElem("right"));
        }
        void ReadArgumentList(EsArr args)
        {
            int argcount = args.Count;
            for (int i = 0; i < argcount; ++i)
            {
                EsElem arg = (EsElem)args[i];
                ReadExpression(arg);

            }
        }
        void ReadNewExpression(EsElem node)
        {
            EsElem newType = node.Expr();
            ReadArgumentList(node.Args());

        }
        void ReadExpressionStatement(EsElem node)
        {
            ReadExpression(node.Expr());

        }
        void ReadFuncDecl(EsElem funcDecl)
        {
            ReadNameSyntax(funcDecl);
            //
            ReadParameters(funcDecl.GetAttributeValueAsArray("parameters"));

            ReadType(funcDecl.Type()); //may be null

            EsElem body = funcDecl.GetAttributeValueAsElem("body");
            //is multiline ?
            if (body != null)
            {
                bool multiLine = body.GetAttributeValueAsBool("multiLine", false);
                EsArr stmts = body.GetAttributeValueAsArray("statements");
                //read each statements
            }
        }
        void ReadInterfaceDecl(EsElem node)
        {
            EsArr members = node.Members();
            int j = members.Count;
            for (int i = 0; i < j; ++i)
            {
                EsElem mb = (EsElem)members[i];
                SyntaxKind mb_kind = mb.Kind();
                switch (mb_kind)
                {
                    default: throw new NotSupportedException();
                    case SyntaxKind.PropertySignature:
                        ReadPropertySignature(mb);
                        break;
                    case SyntaxKind.MethodSignature:
                        //TODO:
                        break;
                    case SyntaxKind.IndexSignature:
                    case SyntaxKind.CallSignature:
                    case SyntaxKind.ConstructSignature:
                        break;
                }
            }
        }
        void ReadEnumDecl(EsElem node)
        {
            ReadNameSyntax(node);
            EsArr members = node.Members();
            int j = members.Count;
            for (int i = 0; i < j; ++i)
            {
                EsElem mb = (EsElem)members[i];
                SyntaxKind mb_kind = mb.Kind();
                switch (mb_kind)
                {
                    default: throw new NotSupportedException();
                    case SyntaxKind.EnumMember:
                        ReadEnumMember(mb);
                        break;
                }
            }
        }
        void ReadEnumMember(EsElem node)
        {
            ReadNameSyntax(node);

        }
        void ReadClassDecl(EsElem node)
        {
            EsArr members = node.Members();
            int j = members.Count;
            for (int i = 0; i < j; ++i)
            {
                EsElem mb = (EsElem)members[i];
                SyntaxKind mb_kind = mb.Kind();
                switch (mb_kind)
                {
                    default: throw new NotSupportedException();
                    case SyntaxKind.PropertyDeclaration:
                        ReadPropertyDecl(mb);
                        break;
                    case SyntaxKind.MethodDeclaration:
                        ReadMethodDecl(mb);
                        break;
                    case SyntaxKind.Constructor:
                        ReadConstructor(mb);
                        break;
                    case SyntaxKind.IndexSignature:
                        break;
                }
            }
        }
        void ReadConstructor(EsElem ctorDecl)
        {
            //modifiers 
            //pars
            ReadParameters(ctorDecl.GetAttributeValueAsArray("parameters"));
            //body 
        }
        void ReadParameters(EsArr pars)
        {
            int j = pars.Count;
            for (int i = 0; i < j; ++i)
            {
                ReadParameter((EsElem)pars[i]);
            }
        }
        void ReadParameter(EsElem par)
        {
            ReadNameSyntax(par);
        }
        void ReadPropertyDecl(EsElem propertyDecl)
        {
            //name,type, modifies
            ReadNameSyntax(propertyDecl);
            ReadType(propertyDecl.Type());
            // 
        }
        void ReadPropertySignature(EsElem propertyDecl)
        {
            //name,type, modifies 
            ReadNameSyntax(propertyDecl);
            ReadType(propertyDecl.Type());
            // 
        }
        void ReadType(EsElem astTypeSyntax)
        {
            if (astTypeSyntax == null)
            {
                return;
            }
            SyntaxKind typeKind = astTypeSyntax.Kind();
            switch (typeKind)
            {
                default:
                    throw new NotSupportedException();
                case SyntaxKind.MappedType:
                case SyntaxKind.ConstructorType:
                    break;
                case SyntaxKind.StringKeyword:
                    break;
                case SyntaxKind.NumberKeyword:
                    break;
                case SyntaxKind.AnyKeyword:
                    break;
                case SyntaxKind.TypeReference:
                    break;
                case SyntaxKind.TypeLiteral:
                    break;
                case SyntaxKind.UnionType:
                    break;
                case SyntaxKind.VoidKeyword:
                    break;
                case SyntaxKind.NeverKeyword:
                    break;
                case SyntaxKind.BooleanKeyword:
                    break;
                case SyntaxKind.ArrayType:
                    break;
                case SyntaxKind.ObjectKeyword:
                    break;
                case SyntaxKind.TupleType:
                    break;
                case SyntaxKind.FunctionType:
                    break;
                case SyntaxKind.IntersectionType:
                    break;
                case SyntaxKind.FirstTypeNode:
                    break;
                case SyntaxKind.LastTypeNode:
                    break;
                case SyntaxKind.TypeQuery:
                    break;
                case SyntaxKind.ParenthesizedType:
                    break;

            }
        }
        void ReadMethodDecl(EsElem methodDecl)
        {

        }
        SharpConnect.Data.EsElem ParseJson(string jsonstr)
        {
            SharpConnect.Data.EaseDocument easedoc = new SharpConnect.Data.EaseDocument();
            return easedoc.Parse(jsonstr);
        }
    }
}