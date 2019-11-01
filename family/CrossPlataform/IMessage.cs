using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace family.CrossPlataform
{
    public interface IMessage
    {
        void ShowToast(string toastString);
        void Vibration(int miliseconds);
    }

}
