using System.Reflection;

namespace maxkr1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var dllLib = Assembly.LoadFile("C:\\Users\\ma32k\\source\\repos\\maxASSs\\maxASSs\\bin\\Debug\\net6.0-windows\\maxASSs.dll");

            string dllPath = "C:\\Users\\ma32k\\source\\repos\\maxASSs\\maxASSs\\bin\\Debug\\net6.0-windows\\maxASSs.dll";

            // Загружаем сборку из файла
            Assembly myAssembly = Assembly.LoadFrom(dllPath);

            // Получаем тип класса из сборки
            Type myType = myAssembly.GetType("maxASSs.Class1");

            // Создаем экземпляр объекта класса
            object myObject = Activator.CreateInstance(myType);

            // Вызываем метод класса
            myType.GetMethod("ShowForm").Invoke(myObject, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}