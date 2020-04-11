using FluentValidation;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HeadManagementSystem.App_Start
{
    public static class FluentConfig
    {
        public static void Register()
        {
            FluentValidationModelValidatorProvider.Configure(x => x.ValidatorFactory = new FluentValidatorCustomResolve());
        }
    }

    public class FluentValidatorCustomResolve : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            if (validatorType == null)
            {
                throw new ArgumentNullException("type");
            }
            return DependencyResolver.Current.GetService(validatorType) as IValidator;
        }
    }
}