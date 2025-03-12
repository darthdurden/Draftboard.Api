using CsvHelper;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Draftboard.Api.Csv
{
    public class CsvInputFormatter : InputFormatter
    {
        public CsvInputFormatter()
        {
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/csv"));
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var type = context.ModelType;
            var request = context.HttpContext.Request;

            var result = await ReadStreamAsync(type, request.Body);
            return await InputFormatterResult.SuccessAsync(result);
        }

        protected override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return IsTypeOfIEnumerable(type);
        }

        private bool IsTypeOfIEnumerable(Type type)
        {
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType == typeof(IList))
                {
                    return true;
                }
            }

            return false;
        }

        protected async Task<object> ReadStreamAsync(Type type, Stream stream)
        {
            Type itemType;
            IList list;
            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments()[0];
                list = (IList)Activator.CreateInstance(type);
            }
            else
            {
                itemType = type.GetElementType();

                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(itemType);

                list = (IList)Activator.CreateInstance(constructedListType);
            }

            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecordsAsync(itemType);
            await foreach(var record in records)
            {
                list.Add(record);
            }
            return list;
        }
    }
}
