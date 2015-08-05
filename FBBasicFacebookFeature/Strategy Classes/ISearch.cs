using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBBasicFacebookFeature
{
    public interface ISearch
    {
        bool Search(string i_NameOfMovie, out IMDb io_TheMovie);
    }
}
