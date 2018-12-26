using Abp.Reflection.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vapps.Infrastructure;

namespace Vapps
{
    public static class TypeExtensions
    {
        private static Type[] s_predefinedTypes;
        private static Type[] s_predefinedGenericTypes;

        static TypeExtensions()
        {
            s_predefinedTypes = new Type[] { typeof(string), typeof(decimal), typeof(DateTime), typeof(TimeSpan), typeof(Guid) };
            s_predefinedGenericTypes = new Type[] { typeof(Nullable<>) };
        }

        public static string AssemblyQualifiedNameWithoutVersion(this Type type)
        {
            string[] strArray = type.AssemblyQualifiedName.Split(new char[] { ',' });
            return string.Format("{0},{1}", strArray[0], strArray[1]);
        }

        public static bool IsSequenceType(this Type seqType)
        {
            return (
                (((seqType != typeof(string))
                && (seqType != typeof(byte[])))
                && (seqType != typeof(char[])))
                && (FindIEnumerable(seqType) != null));
        }

        public static bool IsPredefinedSimpleType(this Type type)
        {
            if ((type.GetTypeInfo().IsPrimitive && (type != typeof(IntPtr))) && (type != typeof(UIntPtr)))
            {
                return true;
            }
            if (type.GetTypeInfo().IsEnum)
            {
                return true;
            }
            return s_predefinedTypes.Any(t => t == type);
            //foreach (Type type2 in s_predefinedTypes)
            //{
            //    if (type2 == type)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        public static bool IsStruct(this Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                return !type.IsPredefinedSimpleType();
            }
            return false;
        }

        public static bool IsPredefinedGenericType(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType)
            {
                type = typeInfo.GetGenericTypeDefinition();
            }
            else
            {
                return false;
            }
            return s_predefinedGenericTypes.Any(t => t == type);
        }

        public static bool IsPredefinedType(this Type type)
        {
            if ((!IsPredefinedSimpleType(type) && !IsPredefinedGenericType(type)) && ((type != typeof(byte[]))))
            {
                return (string.Compare(type.FullName, "System.Xml.Linq.XElement", StringComparison.Ordinal) == 0);
            }
            return true;
        }

        public static bool IsInteger(this Type type)
        {

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNullable(this Type type)
        {
            return type != null && type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsNullAssignable(this Type type)
        {
            return !type.GetTypeInfo().IsValueType || type.IsNullable();
        }

        public static bool IsConstructable(this Type type)
        {
            Guard.ArgumentNotNull(type, "type");

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsAbstract || typeInfo.IsInterface || typeInfo.IsArray || typeInfo.IsGenericTypeDefinition || type == typeof(void))
                return false;

            if (!HasDefaultConstructor(type))
                return false;

            return true;
        }

        [DebuggerStepThrough]
        public static bool IsAnonymous(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType)
            {
                var d = typeInfo.GetGenericTypeDefinition().GetTypeInfo();
                if (d.IsClass && d.IsSealed && d.Attributes.HasFlag(TypeAttributes.NotPublic))
                {
                    var attributes = d.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false);
                    if (attributes != null && attributes.Count() > 0)
                    {
                        //WOW! We have an anonymous type!!!
                        return true;
                    }
                }
            }
            return false;
        }

        [DebuggerStepThrough]
        public static bool HasDefaultConstructor(this Type type)
        {
            Guard.ArgumentNotNull(() => type);
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsValueType)
                return true;

            return typeInfo.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Any(ctor => ctor.GetParameters().Length == 0);
        }

        public static bool IsSubClass(this Type type, Type check)
        {
            Type implementingType;
            return IsSubClass(type, check, out implementingType);
        }

        public static bool IsSubClass(this Type type, Type check, out Type implementingType)
        {
            Guard.ArgumentNotNull(type, "type");
            Guard.ArgumentNotNull(check, "check");

            return IsSubClassInternal(type, type, check, out implementingType);
        }

        private static bool IsSubClassInternal(Type initialType, Type currentType, Type check, out Type implementingType)
        {
            var initialTypeInfo = initialType.GetTypeInfo();
            var currentTypeInfo = currentType.GetTypeInfo();
            if (currentType == check)
            {
                implementingType = currentType;
                return true;
            }

            // don't get interfaces for an interface unless the initial type is an interface
            if (check.GetTypeInfo().IsInterface && (initialTypeInfo.IsInterface || currentType == initialType))
            {
                foreach (Type t in currentType.GetInterfaces())
                {
                    if (IsSubClassInternal(initialType, t, check, out implementingType))
                    {
                        // don't return the interface itself, return it's implementor
                        if (check == implementingType)
                            implementingType = currentType;

                        return true;
                    }
                }
            }

            if (currentTypeInfo.IsGenericType && !currentTypeInfo.IsGenericTypeDefinition)
            {
                if (IsSubClassInternal(initialType, currentType.GetGenericTypeDefinition(), check, out implementingType))
                {
                    implementingType = currentType;
                    return true;
                }
            }

            if (currentTypeInfo.BaseType == null)
            {
                implementingType = null;
                return false;
            }

            return IsSubClassInternal(initialType, currentTypeInfo.BaseType, check, out implementingType);
        }

        public static bool IsIndexed(this PropertyInfo property)
        {
            Guard.ArgumentNotNull(property, "property");
            return !property.GetIndexParameters().IsNullOrEmpty();
        }

        /// <summary>
        /// Determines whether the member is an indexed property.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>
        /// 	<c>true</c> if the member is an indexed property; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIndexed(this MemberInfo member)
        {
            Guard.ArgumentNotNull(member, "member");

            PropertyInfo propertyInfo = member as PropertyInfo;

            if (propertyInfo != null)
                return propertyInfo.IsIndexed();
            else
                return false;
        }

        /// <summary>
        /// Checks to see if the specified type is assignable.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsType<TType>(this Type type)
        {
            return typeof(TType).IsAssignableFrom(type);
        }

        public static string GetNameAndAssemblyName(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            Guard.ArgumentNotNull(type, "type");
            return typeInfo.FullName + ", " + type.GetAssembly().GetName().Name;
        }

        public static IEnumerable<MemberInfo> GetFieldsAndProperties(this Type type, BindingFlags bindingAttr)
        {
            foreach (var fi in type.GetFields(bindingAttr))
            {
                yield return fi;
            }

            foreach (var pi in type.GetProperties(bindingAttr))
            {
                yield return pi;
            }
        }

        public static List<MemberInfo> FindMembers(this Type targetType, MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria)
        {
            Guard.ArgumentNotNull(targetType, "targetType");
            var targetTypeInfo = targetType.GetTypeInfo();

            List<MemberInfo> memberInfos = new List<MemberInfo>(targetTypeInfo.FindMembers(memberType, bindingAttr, filter, filterCriteria));

            // fix weirdness with FieldInfos only being returned for the current Type
            // find base type fields and add them to result
            if ((memberType & MemberTypes.Field) != 0
              && (bindingAttr & BindingFlags.NonPublic) != 0)
            {
                // modify flags to not search for public fields
                BindingFlags nonPublicBindingAttr = bindingAttr ^ BindingFlags.Public;

                while ((targetType = targetTypeInfo.BaseType) != null)
                {
                    memberInfos.AddRange(targetTypeInfo.FindMembers(MemberTypes.Field, nonPublicBindingAttr, filter, filterCriteria));
                }
            }

            return memberInfos;
        }

        //public static Type MakeGenericType(this Type genericTypeDefinition, params Type[] innerTypes)
        //{
        //    Guard.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
        //    Guard.ArgumentNotEmpty<Type>(innerTypes, "innerTypes");
        //    Guard.Argument.IsTrue(genericTypeDefinition.IsGenericTypeDefinition, "genericTypeDefinition", "Type '{0}' must be a generic type definition.".FormatInvariant(genericTypeDefinition));

        //    return genericTypeDefinition.MakeGenericType(innerTypes);
        //}

        public static object CreateGeneric(this Type genericTypeDefinition, Type innerType, params object[] args)
        {
            return CreateGeneric(genericTypeDefinition, new Type[] { innerType }, args);
        }

        public static object CreateGeneric(this Type genericTypeDefinition, Type[] innerTypes, params object[] args)
        {
            return CreateGeneric(genericTypeDefinition, innerTypes, (t, a) => Activator.CreateInstance(t, args));
        }

        public static object CreateGeneric(this Type genericTypeDefinition, Type[] innerTypes, Func<Type, object[], object> instanceCreator, params object[] args)
        {
            Guard.ArgumentNotNull(() => genericTypeDefinition);
            Guard.ArgumentNotEmpty(() => innerTypes);
            Guard.ArgumentNotNull(() => instanceCreator);

            Type specificType = genericTypeDefinition.MakeGenericType(innerTypes);

            return instanceCreator(specificType, args);
        }

        public static IList CreateGenericList(this Type listType)
        {
            Guard.ArgumentNotNull(listType, "listType");
            return (IList)typeof(List<>).CreateGeneric(listType);
        }

        //public static Type RemoveNullable(this Type type)
        //{
        //    if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
        //    {
        //        return type.GetGenericArguments()[0];
        //    }
        //    return type;
        //}

        public static bool IsEnumerable(this Type type)
        {
            Guard.ArgumentNotNull(type, "type");
            return type.IsAssignableFrom(typeof(IEnumerable));
        }

        public static bool IsGenericDictionary(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsInterface && typeInfo.IsGenericType)
            {
                return typeof(IDictionary<,>).Equals(typeInfo.GetGenericTypeDefinition());
            }
            return (typeInfo.GetInterface(typeof(IDictionary<,>).Name) != null);
        }

        /// <summary>
        /// Gets the underlying type of a <see cref="Nullable{T}" /> type.
        /// </summary>
        public static Type GetNonNullableType(this Type type)
        {
            if (!IsNullable(type))
            {
                return type;
            }
            return type.GetGenericArguments()[0];
        }

        /// <summary>
        /// Determines whether the specified MemberInfo can be read.
        /// </summary>
        /// <param name="member">The MemberInfo to determine whether can be read.</param>
        /// <returns>
        /// 	<c>true</c> if the specified MemberInfo can be read; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// For methods this will return <c>true</c> if the return type
        /// is not <c>void</c> and the method is parameterless.
        /// </remarks>
        public static bool CanReadValue(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return true;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).CanRead;
                case MemberTypes.Method:
                    MethodInfo mi = (MethodInfo)member;
                    return mi.ReturnType != typeof(void) && mi.GetParameters().Length == 0;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the specified MemberInfo can be set.
        /// </summary>
        /// <param name="member">The MemberInfo to determine whether can be set.</param>
        /// <returns>
        /// 	<c>true</c> if the specified MemberInfo can be set; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanSetValue(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return true;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).CanWrite;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns single attribute from the type
        /// </summary>
        /// <typeparam name="T">Attribute to use</typeparam>
        /// <param name="target">Attribute provider</param>
        ///<param name="inherit"><see cref="MemberInfo.GetCustomAttributes(Type,bool)"/></param>
        /// <returns><em>Null</em> if the attribute is not found</returns>
        /// <exception cref="InvalidOperationException">If there are 2 or more attributes</exception>
        public static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider target, bool inherits) where TAttribute : Attribute
        {
            if (target.IsDefined(typeof(TAttribute), inherits))
            {
                var attributes = target.GetCustomAttributes(typeof(TAttribute), inherits);
                if (attributes.Length > 1)
                {
                    throw Error.MoreThanOneElement();
                }
                return (TAttribute)attributes[0];
            }

            return null;

        }

        public static bool HasAttribute<TAttribute>(this ICustomAttributeProvider target, bool inherits) where TAttribute : Attribute
        {
            return target.IsDefined(typeof(TAttribute), inherits);
        }

        /// <summary>
        /// Given a particular MemberInfo, return the custom attributes of the
        /// given type on that member.
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute to retrieve.</typeparam>
        /// <param name="member">The member to look at.</param>
        /// <param name="inherits">True to include attributes inherited from base classes.</param>
        /// <returns>Array of found attributes.</returns>
        public static TAttribute[] GetAttributes<TAttribute>(this ICustomAttributeProvider target, bool inherits) where TAttribute : Attribute
        {
            if (target.IsDefined(typeof(TAttribute), inherits))
            {
                var attributes = target
                    .GetCustomAttributes(typeof(TAttribute), inherits)
                    .Cast<TAttribute>();

                return SortAttributesIfPossible(attributes).ToArray();

                #region Obsolete
                //return target
                //    .GetCustomAttributes(typeof(TAttribute), inherits)
                //    .ToArray(a => (TAttribute)a);
                #endregion
            }
            return new TAttribute[0];

            #region Obsolete
            //// OBSOLETE 1
            //return target.GetCustomAttributes(typeof(TAttribute), inherits).Cast<TAttribute>().ToArray();

            //// OBSOLETE 2
            //object[] attributesAsObjects = member.GetCustomAttributes(typeof(TAttribute), inherits);
            //TAttribute[] attributes = new TAttribute[attributesAsObjects.Length];
            //int index = 0;
            //Array.ForEach(attributesAsObjects,
            //    delegate(object o)
            //    {
            //        attributes[index++] = (TAttribute)o;
            //    });
            //return attributes;
            #endregion
        }

        /// <summary>
        /// Given a particular MemberInfo, find all the attributes that apply to this
        /// member. Specifically, it returns the attributes on the type, then (if it's a
        /// property accessor) on the property, then on the member itself.
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute to retrieve.</typeparam>
        /// <param name="member">The member to look at.</param>
        /// <param name="inherits">true to include attributes inherited from base classes.</param>
        /// <returns>Array of found attributes.</returns>
        public static TAttribute[] GetAllAttributes<TAttribute>(this MemberInfo member, bool inherits)
            where TAttribute : Attribute
        {
            List<TAttribute> attributes = new List<TAttribute>();

            if (member.DeclaringType != null)
            {
                attributes.AddRange(GetAttributes<TAttribute>(member, inherits));

                MethodBase methodBase = member as MethodBase;
                if (methodBase != null)
                {
                    PropertyInfo prop = GetPropertyFromMethod(methodBase);
                    if (prop != null)
                    {
                        attributes.AddRange(GetAttributes<TAttribute>(prop, inherits));
                    }
                }
            }
            attributes.AddRange(GetAttributes<TAttribute>(member, inherits));
            return attributes.ToArray();
        }

        internal static IEnumerable<TAttribute> SortAttributesIfPossible<TAttribute>(IEnumerable<TAttribute> attributes)
            where TAttribute : Attribute
        {
            if (typeof(IOrdered).IsAssignableFrom(typeof(TAttribute)))
            {
                return attributes.Cast<IOrdered>().OrderBy(x => x.Ordinal).Cast<TAttribute>();
            }

            return attributes;
        }

        /// <summary>
        /// Given a MethodBase for a property's get or set method,
        /// return the corresponding property info.
        /// </summary>
        /// <param name="method">MethodBase for the property's get or set method.</param>
        /// <returns>PropertyInfo for the property, or null if method is not part of a property.</returns>
        public static PropertyInfo GetPropertyFromMethod(this MethodBase method)
        {
            Guard.ArgumentNotNull(method, "method");

            PropertyInfo property = null;
            if (method.IsSpecialName)
            {
                Type containingType = method.DeclaringType;
                if (containingType != null)
                {
                    if (method.Name.StartsWith("get_") ||
                        method.Name.StartsWith("set_"))
                    {
                        string propertyName = method.Name.Substring(4);
                        property = containingType.GetProperty(propertyName);
                    }
                }
            }
            return property;
        }

        internal static Type FindIEnumerable(this Type seqType)
        {
            var seqTypeInfo = seqType.GetTypeInfo();

            if (seqType == null || seqType == typeof(string))
                return null;
            if (seqType.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(seqTypeInfo.GetElementType());
            if (seqTypeInfo.IsGenericType)
            {
                foreach (Type arg in seqTypeInfo.GetGenericArguments())
                {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType))
                        return ienum;
                }
            }
            Type[] ifaces = seqTypeInfo.GetInterfaces();
            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (Type iface in ifaces)
                {
                    Type ienum = FindIEnumerable(iface);
                    if (ienum != null)
                        return ienum;
                }
            }
            if (seqTypeInfo.BaseType != null && seqTypeInfo.BaseType != typeof(object))
                return FindIEnumerable(seqTypeInfo.BaseType);
            return null;
        }
    }
}
