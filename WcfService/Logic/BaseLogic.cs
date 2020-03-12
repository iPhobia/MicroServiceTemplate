using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Logic
{
    public abstract class BaseLogic
    {
        protected DbGateway Gateway;
        protected DbGatewayADO GatewayADO;

        protected void Try(Action act)
        {
            using(Gateway = new DbGateway())
            {
                try
                {
                    act();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        protected void TryADO(Action act)
        {
            using(GatewayADO = new DbGatewayADO())
            {
                try
                {
                    act();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}