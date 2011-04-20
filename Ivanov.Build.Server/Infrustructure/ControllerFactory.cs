﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;

namespace Ivanov.Build.Server.Infrustructure
{
    public class ControllerFactory : DefaultControllerFactory
    {
        private IKernel _kernel = new StandardKernel(new Services());

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }

            return _kernel.Get(controllerType) as IController;
        }
    }

}