using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Kogane
{
    /// <summary>
    /// リフレクションでクラスや構造体のすべてのフィールドとプロパティの情報を取得するためのクラス
    /// </summary>
    public static class ReflectionMembersUtils
    {
        //==============================================================================
        // 関数(static)
        //==============================================================================
        /// <summary>
        /// すべてのフィールドの情報を返します
        /// </summary>
        public static Dictionary<string, object> GetFields<T>
        (
            T    self,
            bool includePublic    = true,
            bool includeNonPublic = true,
            bool includeStatic    = false
        )
        {
            var bindingAttr = GetBindingAttr
            (
                includePublic: includePublic,
                includeNonPublic: includeNonPublic,
                includeStatic: includeStatic
            );

            return self
                    .GetType()
                    .GetFields( bindingAttr )
                    .Where( x => !IsBackingField( x ) )
                    .ToDictionary( x => x.Name, x => x.GetValue( self ) )
                ;
        }

        /// <summary>
        /// すべてのプロパティの情報を返します
        /// </summary>
        public static Dictionary<string, object> GetProperties<T>
        (
            T    self,
            bool includePublic    = true,
            bool includeNonPublic = true,
            bool includeStatic    = false
        )
        {
            var bindingAttr = GetBindingAttr
            (
                includePublic: includePublic,
                includeNonPublic: includeNonPublic,
                includeStatic: includeStatic
            );

            return self
                    .GetType()
                    .GetProperties( bindingAttr )
                    .ToDictionary( x => x.Name, x => x.GetValue( self ) )
                ;
        }

        /// <summary>
        /// すべてのフィールドとプロパティの情報を返します
        /// </summary>
        public static Dictionary<string, object> GetFieldsAndProperties<T>
        (
            T    self,
            bool includePublic    = true,
            bool includeNonPublic = true,
            bool includeStatic    = false
        )
        {
            var fields = GetFields
            (
                self: self,
                includePublic: includePublic,
                includeNonPublic: includeNonPublic,
                includeStatic: includeStatic
            );

            var properties = GetProperties
            (
                self: self,
                includePublic: includePublic,
                includeNonPublic: includeNonPublic,
                includeStatic: includeStatic
            );

            foreach ( var property in properties )
            {
                fields.Add( property.Key, property.Value );
            }

            return fields;
        }

        /// <summary>
        /// 指定された条件に紐づく BindingFlags を作成して返します
        /// </summary>
        private static BindingFlags GetBindingAttr
        (
            bool includePublic,
            bool includeNonPublic,
            bool includeStatic
        )
        {
            var bindingAttr = BindingFlags.Instance;

            if ( includePublic ) bindingAttr    |= BindingFlags.Public;
            if ( includeNonPublic ) bindingAttr |= BindingFlags.NonPublic;
            if ( includeStatic ) bindingAttr    |= BindingFlags.Static;

            return bindingAttr;
        }

        /// <summary>
        /// バッキングフィールドの場合 true を返します
        /// </summary>
        public static bool IsBackingField( FieldInfo field )
        {
            return field.IsDefined( typeof( CompilerGeneratedAttribute ), false );
        }

        /// <summary>
        /// 自動実装プロパティによって追加されたバッキングフィールドの場合 true を返します
        /// </summary>
        public static bool IsPropertyBackingField( FieldInfo field )
        {
            return IsBackingField( field ) && field.Name[ 0 ] == '<';
        }

        /// <summary>
        /// 実装を省略した event によって追加されたバッキングフィールドの場合 true を返します
        /// </summary>
        public static bool IsEventBackingField( FieldInfo field )
        {
            return IsBackingField( field ) && field.Name[ 0 ] != '<';
        }
    }
}