using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using family.Domain.Dto;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using Xamarin.Forms;

namespace family.Model
{
    public class ModelPushNotification
    {
        private App _app => (Application.Current as App);

        private ModelUsuario bllConexao = new ModelUsuario();

        private CancellationToken paramTokenCancel { get; set; }

        private String paramUserID { get; set; }

        private String paramPushToken { get; set; }

        public async void DeletePushKey(CancellationToken paramToken)
        {
            try
            {

                await Push.SetEnabledAsync(false);

                OneSignal.Current.SetSubscription(false);
                await bllConexao.Atualiza_PushKey(
                    ""
                    , paramToken
                );

            }
            catch (Exception) { }
        }

        private void IdsAvailable(string userID, string pushToken)
        {
            OneSignal.Current.SetSubscription(true);

            paramUserID = userID;

            // ***** Gera guid personalizado para o OneSignal
            //Guid g;
            //g = Guid.NewGuid();
            //OneSignal.Current.SetExternalUserId(g.ToString());
            //userID = g.ToString();

            bllConexao.Atualiza_PushKey(userID, paramTokenCancel);

        }

        public async void RegistraPushKey(CancellationToken paramToken)
        {
            paramTokenCancel = paramToken;

            try
            {
                String strChave = String.Empty;


                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {

                    await Push.SetEnabledAsync(true);

                    System.Guid? installId = await AppCenter.GetInstallIdAsync();

                    if (_app.nameProject == "family")
                    {
                        //strChave = "eb3e8927-71b4-497d-b4d6-3366944d4812";
                        strChave = "6fe8e8b4-8ba4-44da-ad77-0ec70508b062";

                    }

                    if (_app.nameProject == "khronos")
                    {
                        strChave = "86dc3125-3232-4046-9d65-698d4d2379e1";
                    }


                    if (_app.nameProject == "maxima")
                    {
                        strChave = "8c81f881-5c5b-43e3-b688-67d51d5f3dad";
                    }


                    if (_app.nameProject == "spywave")
                    {
                        strChave = "93bce76e-96f7-40ef-b081-6e97888dc1e3";
                    }

                    if (_app.nameProject == "alltech")
                    {
                        strChave = "013e9738-7d25-42ad-a3df-5011a899b7b4";
                    }


                    bllConexao.Atualiza_PushKey(installId.ToString(), paramToken);

                }
                else
                {


                    if (_app.nameProject == "family")
                        strChave = "dbe55559-99e9-487c-9b98-2f93fcff459f";

                    if (_app.nameProject == "khronos")
                        strChave = "70f5857d-325d-4612-8e59-6b061fc95b34";

                    if (_app.nameProject == "maxima")
                        strChave = "9aea4c81-4ed0-426b-a809-8e2d8ecc44c2";

                    if (_app.nameProject == "2minutos")
                        strChave = "58933542-b4be-4059-b8a2-a8c98d51c01a";

                    if (_app.nameProject == "spywave")
                        strChave = "a9faf7e2-2fa3-4f37-959c-0a0275d05ca8";

                    if (_app.nameProject == "atm")
                        strChave = "2737ba84-8257-406b-890e-5f9192840e6f";



                    OneSignal.Current.StartInit(strChave)
                             .Settings(new Dictionary<string, bool>() {
                    { IOSSettings.kOSSettingsKeyAutoPrompt, true },
                    { IOSSettings.kOSSettingsKeyInAppLaunchURL, true } })
                             .InFocusDisplaying(OSInFocusDisplayOption.Notification)
                             .HandleNotificationOpened((result) =>
                             {
                                 //Debug.WriteLine("HandleNotificationOpened: {0}", result.notification.payload.body);
                             })
                             .HandleNotificationReceived((notification) =>
                             {
                                 //Debug.WriteLine("HandleNotificationReceived: {0}", notification.payload.body);
                             })
                             .EndInit();


                    //OneSignal.Current.RegisterForPushNotifications();

                    OneSignal.Current.IdsAvailable(IdsAvailable);

                    //bllConexao.Atualiza_PushKey(paramUserID, paramToken);


                }


            }
            catch (Exception) { }
        }

        public async Task<ServiceResult<String>> GetRegistraPushKey()
        {

            ServiceResult<String> resultPush = new ServiceResult<String>();

            try
            {
                String strChave = String.Empty;


                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {

                    await Push.SetEnabledAsync(true);

                    System.Guid? installId = await AppCenter.GetInstallIdAsync();


                    if (_app.nameProject == "family")
                    {
                        //strChave = "eb3e8927-71b4-497d-b4d6-3366944d4812";
                        strChave = "6fe8e8b4-8ba4-44da-ad77-0ec70508b062";

                    }

                    if (_app.nameProject == "khronos")
                    {
                        strChave = "86dc3125-3232-4046-9d65-698d4d2379e1";
                    }


                    if (_app.nameProject == "maxima")
                    {
                        strChave = "8c81f881-5c5b-43e3-b688-67d51d5f3dad";
                    }


                    if (_app.nameProject == "spywave")
                    {
                        strChave = "";
                    }

                    resultPush.Data = installId.ToString();
                }
                else
                {

                    strChave = "dbe55559-99e9-487c-9b98-2f93fcff459f";


                    if (_app.isPersonalizado)
                    {
                        if (_app.nameProject == "khronos")
                            strChave = "70f5857d-325d-4612-8e59-6b061fc95b34";

                        if (_app.nameProject == "maxima")
                            strChave = "9aea4c81-4ed0-426b-a809-8e2d8ecc44c2";

                        if (_app.nameProject == "2minutos")
                            strChave = "58933542-b4be-4059-b8a2-a8c98d51c01a";

                        if (_app.nameProject == "spywave")
                            strChave = "a9faf7e2-2fa3-4f37-959c-0a0275d05ca8";

                    }



                    OneSignal.Current.StartInit(strChave)
                             .Settings(new Dictionary<string, bool>() {
                    { IOSSettings.kOSSettingsKeyAutoPrompt, true },
                    { IOSSettings.kOSSettingsKeyInAppLaunchURL, true } })
                             .InFocusDisplaying(OSInFocusDisplayOption.Notification)
                             .HandleNotificationOpened((result) =>
                             {
                                 //Debug.WriteLine("HandleNotificationOpened: {0}", result.notification.payload.body);
                             })
                             .HandleNotificationReceived((notification) =>
                             {
                                 //Debug.WriteLine("HandleNotificationReceived: {0}", notification.payload.body);
                             })
                             .EndInit();


                    OneSignal.Current.IdsAvailable(IdsAvailable);

                    resultPush.Data = paramUserID;


                }


            }
            catch (Exception) { }

            return resultPush;
        }


    }
}
