# Kogane Reflection Members Utils

リフレクションでクラスや構造体のすべてのフィールドとプロパティの情報を取得するクラス

## 使用例

```csharp
using System.Diagnostics.CodeAnalysis;
using Kogane;
using UnityEngine;

public class Character
{
    private readonly int    m_id;
    private readonly string m_name;

    private int    Id   => m_id;
    private string Name => m_name;
}

public class Example : MonoBehaviour
{
    private void Start()
    {
        var character = new Character();

        var fields = ReflectionMembersUtils.GetFields
        (
            character,
            includePublic: true,
            includeNonPublic: true,
            includeStatic: false
        );

        foreach ( var x in fields )
        {
            Debug.Log( $"{x.Key}: {x.Value}" );
        }

        var properties = ReflectionMembersUtils.GetProperties
        (
            character,
            includePublic: true,
            includeNonPublic: true,
            includeStatic: false
        );

        foreach ( var x in properties )
        {
            Debug.Log( $"{x.Key}: {x.Value}" );
        }

        var fieldsAndProperties = ReflectionMembersUtils.GetFieldsAndProperties
        (
            character,
            includePublic: true,
            includeNonPublic: true,
            includeStatic: false
        );

        foreach ( var x in fieldsAndProperties )
        {
            Debug.Log( $"{x.Key}: {x.Value}" );
        }
    }
}
```