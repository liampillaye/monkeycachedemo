using System;
using MonkeyCache;
using MonkeyCacheDemo.Cache;
using Xamarin.Forms;
using BarrelFile = MonkeyCache.FileStore.Barrel;
using BarrelLite = MonkeyCache.LiteDB.Barrel;
using BarrelSQL = MonkeyCache.SQLite.Barrel;
using System.Data.SqlTypes;

namespace MonkeyCacheDemo
{
    public partial class MainPage : ContentPage
    {
        #region Privates
        private IBarrel _sql;
        private IBarrel _file;
        private IBarrel _lite;
        private readonly string _tKey = "cache22";
        private readonly string _applicationIdLiteDB = "com.refractored.monkeycachetestlite";
        private readonly string _applicationIdSqlite = "com.refractored.monkeycachetestsql";
        private readonly string _applicationIdFileStore = "com.refractored.monkeycachetestfile";
        #endregion

        public MainPage()
        {
            InitializeComponent();

            //SetupCache
            BarrelLite.ApplicationId = _applicationIdLiteDB;
            BarrelFile.ApplicationId = _applicationIdFileStore;
            BarrelSQL.ApplicationId = _applicationIdSqlite;
            _sql = BarrelSQL.Current;
            _lite = BarrelLite.Current;
            _file = BarrelFile.Current;

            //Event Handlers
            ButtonLoad.Clicked += ButtonLoad_Clicked;
            ButtonSave.Clicked += ButtonSave_Clicked;
            ButtonExpired.Clicked += ButtonExpired_Clicked;
        }

        private IBarrel GetCurrent()
        {
            IBarrel current = null;

            if (UseFileStore.IsToggled)
                current = _file;
            else if (UseLiteDB.IsToggled)
                current = _lite;
            else
                current = _sql;

            return current;
        }

        #region EventHandlers
        private void ButtonSave_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstName.Text) || string.IsNullOrWhiteSpace(LastName.Text))
            {
                DisplayAlert("Info", "Please enter a fullname", "OK");
                return;
            }
            var cache22 = new Cache22 { FirstName = FirstName.Text, LastName = LastName.Text };
            GetCurrent().Add(_tKey, cache22, TimeSpan.FromMinutes(5.0));
            DisplayAlert(":)", "Saved!", "OK");
        }

        private void ButtonLoad_Clicked(object sender, EventArgs e)
        {
            var cache22 = GetCurrent().Get<Cache22>(_tKey);
            if (cache22 == null)
                DisplayAlert(";(", "Cache22", "OK");
            else
            {
                DisplayAlert(":)", $"Hi {cache22.FirstName} {cache22.LastName}", "OK");
                var expired = GetCurrent().GetExpiration(_tKey);
            }
        }

        private void ButtonExpired_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Is Expired?", GetCurrent().IsExpired(_tKey) ? "Yes" : "No", "OK");
        }
        #endregion
    }
}
