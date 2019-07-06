﻿namespace Alphacloud.MessagePack.AspNetCore.Formatters
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::MessagePack;
    using Internal;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Mvc.Formatters;


    /// <summary>
    ///     MVC Input formatter.
    /// </summary>
    [PublicAPI]
    public class MessagePackInputFormatter : InputFormatter
    {
        readonly IFormatterResolver _resolver;
        readonly ReadableTypesCache _readableTypesCache;

        /// <inheritdoc />
        public MessagePackInputFormatter([NotNull] IFormatterResolver resolver, [NotNull] ICollection<string> mediaTypes)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            if (mediaTypes == null) throw new ArgumentNullException(nameof(mediaTypes));
            if (mediaTypes.Count == 0) throw new ArgumentException("Media type must be specified.", nameof(mediaTypes));
            _readableTypesCache = new ReadableTypesCache(resolver);

            foreach (var mediaType in mediaTypes)
            {
                SupportedMediaTypes.Add(mediaType);
            }
        }

        /// <inheritdoc />
        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            var result = MessagePackSerializer.NonGeneric.Deserialize(context.ModelType, request.Body, _resolver);
            return InputFormatterResult.SuccessAsync(result);
        }

        /// <inheritdoc />
        protected override bool CanReadType(Type type)
        {
            return _readableTypesCache.CanRead(type);
        }
    }
}