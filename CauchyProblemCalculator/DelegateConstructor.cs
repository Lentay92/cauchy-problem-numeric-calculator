using System;
using System.Text;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace CauchyProblem
{
    public class DelegateConstructor
    {
        #region static variables
        // Статическая переменная для создания уникальных имен
        private static Int32 m_classIndex = 0;
        #endregion


        #region internal types
        /// <summary>
        /// Класс, который содержит всю информацию о делегате
        /// </summary>
        public class DelegateInfo<T>
        {
            // Исходный код функции
            public String Code { get; set; }
            // Информация о созданной функции
            public MethodInfo MethodInfo { get; set; }
            // Скомпилированный делегат
            public Delegate Delegate { get; set; }
            public T Exec { get { return (T)Convert.ChangeType(this.Delegate, typeof(T)); } }
            // Текст ошибки
            public String ErrorText { get; set; }
            // Была ли ошибка компиляции
            public bool WasError { get { return !String.IsNullOrEmpty(this.ErrorText); } }
        }
        #endregion


        #region management
        /// <summary>
        /// Создаем делегат по коду
        /// </summary>
        /// <typeparam name="T"> Тип делегата </typeparam>
        /// <param name="name"> Имя создаваемой функции. Должно быть таким же, как в тексте функции </param>
        /// <param name="code"> Текст функции</param>
        public static T CreateDelegate<T>(String funcName, String code)
        {
            // Заполняем информацию о функции
            DelegateInfo<T> del = new DelegateInfo<T>();
            del.Code = code;
            del.MethodInfo = null;
            del.Delegate = null;
            del.ErrorText = String.Empty;

            // Компилируем функцию
            CompileDelegate<T>(funcName, del);
            if (!del.WasError)
                return (T)Convert.ChangeType(del.Delegate, typeof(T));
            return default(T);
        }


        /// <summary>
        /// Создаем делегат по коду и возвращаем структуру с информацией о нем
        /// </summary>
        /// <typeparam name="T"> Тип делегата </typeparam>
        /// <param name="name"> Имя создаваемой функции. Должно быть таким же, как в тексте функции </param>
        /// <param name="code"> Текст функции </param>
        public static DelegateInfo<T> CreateDelegateInfo<T>(String funcName, String code)
        {
            // Заполняем информацию о функции
            DelegateInfo<T> del = new DelegateInfo<T>();
            del.Code = code;
            del.MethodInfo = null;
            del.Delegate = null;
            del.ErrorText = String.Empty;

            // Компилируем функцию
            CompileDelegate<T>(funcName, del);
            return del;
        }
        #endregion


        #region compile functions
        /// <summary>
        /// Компилируем делегат
        /// </summary>
        private static void CompileDelegate<T>(String name, DelegateInfo<T> del)
        {
            // Перечисляем все библиотеки, от которых может зависеть текст функции
            String[] referenceAssemblies =
            {
                            "System.dll",
                            "System.Data.dll",
                            "System.Design.dll",
                            "System.Drawing.dll",
                            "System.Windows.Forms.dll",
                            "System.Xml.dll",
                            "System.Runtime.dll"
            };

            String className = "C" + name + m_classIndex.ToString();
            m_classIndex++;

            // Создаем полный текст класса с функцией
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using System.Design;");
            sb.AppendLine("using System.Drawing;");
            sb.AppendLine("using System.Windows.Forms;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("namespace DelegateGenerator");
            sb.AppendLine("{");
            sb.Append("    public class ");
            sb.AppendLine(className);
            sb.AppendLine("        {");
            sb.AppendLine(del.Code);
            sb.AppendLine("        }");
            sb.AppendLine("}");

            // Компилируем класс
            CompilerParameters codeParams = new CompilerParameters(referenceAssemblies);
            codeParams.GenerateExecutable = false;
            codeParams.GenerateInMemory = true;
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CompilerResults codeResult = codeProvider.CompileAssemblyFromSource(codeParams, sb.ToString());

            // Проверяем результат на ошибку
            if (codeResult.Errors.HasErrors)
            {
                StringBuilder err = new StringBuilder();
                for (int i = 0; i < codeResult.Errors.Count; ++i)
                    err.AppendLine(codeResult.Errors[i].ToString());
                del.ErrorText = err.ToString();
                return;
            }

            // Получаем функцию созданного класса по имени
            Type type = codeResult.CompiledAssembly.GetType("DelegateGenerator." + className);
            del.MethodInfo = type.GetMethod(name);
            if (null == del.MethodInfo)
            {
                del.ErrorText = String.Format("Delegate name '{0}' error", name);
            }
            else
            {
                del.Delegate = Delegate.CreateDelegate(typeof(T), del.MethodInfo);
                if (null == del.Delegate)
                    del.ErrorText = String.Format("Delegate type '{0}' error", name);
            }
        }
        #endregion

        public delegate Single functionDelegate(Single x, Single y);

        public static functionDelegate MakeDelegate(String funcText)
        {
            funcText = "public static Single FuncName(Single x, Single y)" +
                        "{" +
                        "       return (" + funcText + ");" +
                        "}";

            functionDelegate function = DelegateConstructor.CreateDelegate<functionDelegate>("FuncName", funcText);

            return function;
        }
    }
}

