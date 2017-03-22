using MedicalAdministrationSystem.DataAccess;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    class Codes
    {
        protected internal MedicalModel me { get; set; }
        protected internal bool workingConn;
        protected internal async Task<string> Generate(int UserId, int PatientId)
        {
            await Loading.Show();
            return await Task.Run(() =>
            {
                try
                {
                    me = new MedicalModel(ConfigurationManager.Connect());
                    workingConn = true;
                    me.Database.Connection.Open();
                    return Generate(
                        (int)me.userdata.Where(a => a.IdUD == UserId).Select(a => a.SealNumberUD).Single(),
                        me.patientdata.Where(b => b.IdPD == PatientId).Select(b => b.TAJNumberPD).Single());
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    workingConn = false;
                    return null;
                }
                finally
                {
                    me.Database.Connection.Close();
                }
            }, CancellationToken.None).ContinueWith(task =>
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(async () =>
                    await Loading.Hide()));
                return task.Result;
            });
        }
        private string Generate(int sealNumber, string TAJ)
        {
            DateTime dt = DateTime.Now;
            TAJ = TAJ.Remove(3, 1);
            TAJ = TAJ.Remove(6, 1);
            int inttaj = Convert.ToInt32(TAJ);
            string id = (inttaj + sealNumber).ToString();
            id = id.Remove(0, id.Length - 5);
            return id + string.Format("{0:yy}", dt) + string.Format("{0:MM}", dt) + dt.Day.ToString("d2") + string.Format("{0:HH}", dt) + string.Format("{0:mm}", dt);
        }
    }
}
