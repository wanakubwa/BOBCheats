﻿using BOBCheats.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOBCheats.GUI
{
    public class CheatsMenuModel : UIModel
    {
        #region Fields



        #endregion

        #region Propeties



        #endregion

        #region Methods

        public void EnableCheat(CheatInfo cheat)
        {
            BOBCheatsManager bOBCheatsManager = BOBCheatsManager.Instance;
            if(bOBCheatsManager != null)
            {
                bOBCheatsManager.UseCheat(cheat);
            }
        }

        public List<CheatInfo> GetAvaibleCheats()
        {
            BOBCheatsManager cheatsManager = BOBCheatsManager.Instance;
            if(cheatsManager == null)
            {
                return null;
            }

            return cheatsManager.CheatsCollection;
        }

        #endregion

        #region Enums



        #endregion
    }
}
