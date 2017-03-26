using MedicalAdministrationSystem.DataAccess;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    class Codes : VMExtender
    {
        protected internal async Task<string> Generate(int UserId, int PatientId)
        {
            await Loading.Show();
            return await Task.Run(async () =>
            {
                try
                {
                    using (me = new MedicalModel(ConfigurationManager.Connect()))
                    {
                        await me.Database.Connection.OpenAsync();
                        string serialnumber = Generate(
                            (int)me.userdata.Where(a => a.IdUD == UserId).Select(a => a.SealNumberUD).Single(),
                            me.patientdata.Where(b => b.IdPD == PatientId).Select(b => b.TAJNumberPD).Single());
                        workingConn = true;
                        return serialnumber;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    workingConn = false;
                    return null;
                }
            }, CancellationToken.None).ContinueWith(task =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(async () =>
                {
                    if (!workingConn) ConnectionMessage();
                    await Loading.Hide();
                }));
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
