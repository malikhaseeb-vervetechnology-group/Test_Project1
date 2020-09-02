using System;
using System.IO;
using SQLite;
using Test_Project1.iOS.Persistence;
using Test_Project1.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqLiteDb))]
namespace Test_Project1.iOS.Persistence
{
    public class SqLiteDb: ISqLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentPath, "Test_Project.db1");

            return new SQLiteAsyncConnection(path);
        }
    }
}