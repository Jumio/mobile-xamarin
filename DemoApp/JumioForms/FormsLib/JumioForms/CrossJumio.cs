using System;
using JumioForms.Abstractions;

namespace JumioForms
{
    public class CrossJumio
    {
        public static INetverify CurrentNetverify
        {
            get
            {
#if __ANDROID__
                if (CurrentActivity == null)
                {
                    throw new Exception("You Must Call Init Method Prior To Use The Library");
                }
#endif
                var ret = ImplementationNetverify.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        public static IDocumentverification CurrentDocumentVerification
        {
            get
            {
                {
#if __ANDROID__
                    if (CurrentActivity == null)
                    {
                        throw new Exception("You Must Call Init Method Prior To Use The Library");
                    }
#endif
                    var ret = ImplementationDocumentVerification.Value;
                    if (ret == null)
                    {
                        throw NotImplementedInReferenceAssembly();
                    }
                    return ret;
                }
            }
        }

        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsNetverifySupported => ImplementationNetverify.Value == null ? false : true;

        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsDocumentverifySupported => ImplementationDocumentVerification.Value == null ? false : true;

        private static Lazy<INetverify> ImplementationNetverify = new Lazy<INetverify>(() => CreateNetverify(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private static Lazy<IDocumentverification> ImplementationDocumentVerification = new Lazy<IDocumentverification>(() => CreateDocumentVerification(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }

#if __ANDROID__

        public static global::Android.App.Activity CurrentActivity { get; private set;  }

        public static void Init(global::Android.App.Activity activity) => CurrentActivity = activity;

#endif

        private static INetverify CreateNetverify()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else
#pragma warning disable IDE0022 // Use expression body for methods
            return new JumioNetverify();
#pragma warning restore IDE0022 // Use expression body for methods
#endif
        }

        private static IDocumentverification CreateDocumentVerification()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else
#pragma warning disable IDE0022 // Use expression body for methods
            return new JumioDocumentVerification();
#pragma warning restore IDE0022 // Use expression body for methods
#endif
        }
    }
}
