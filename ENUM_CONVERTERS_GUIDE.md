# Enum Value Converters Guide

## Current Implementation

Currently, your EF Core configuration uses `.HasConversion<string>()` which stores enum values as their PascalCase names (e.g., "InProgress", "SpecialOrder", "FloatAdjustment").

The corrected SQL schema uses `VARCHAR` columns which will store these PascalCase values correctly.

## If You Want to Use SQL ENUM Types

If you prefer to use SQL `ENUM` types in your database for better data integrity, you'll need to add value converters to your `DbContext` that convert between C# PascalCase enum names and SQL UPPER_SNAKE_CASE values.

### Example Converter Implementation

Here's how you would add converters for the `OrderStatus` enum:

```csharp
// In PcHouseStoreDbContext.OnModelCreating()

modelBuilder.Entity<Order>()
    .Property(o => o.Status)
    .HasConversion(
        v => v.ToString().ToUpperInvariant().Replace("InProgress", "IN_PROGRESS")
                                          .Replace("SpecialOrder", "SPECIAL_ORDER"),
        v => Enum.Parse<OrderStatus>(v.Replace("IN_PROGRESS", "InProgress")
                                      .Replace("SPECIAL_ORDER", "SpecialOrder"))
    );
```

### Better Approach: Create Reusable Converter

Create a helper class for enum conversions:

```csharp
using System.Text.RegularExpressions;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Infrastructure.Data.Converters;

public static class EnumConverters
{
    public static string ToSqlEnum<T>(T enumValue) where T : Enum
    {
        var str = enumValue.ToString();
        // Convert PascalCase to UPPER_SNAKE_CASE
        return Regex.Replace(str, "(?<!^)([A-Z])", "_$1", RegexOptions.Compiled)
                   .ToUpperInvariant();
    }

    public static T FromSqlEnum<T>(string sqlValue) where T : struct, Enum
    {
        // Convert UPPER_SNAKE_CASE to PascalCase
        var parts = sqlValue.ToLowerInvariant().Split('_');
        var pascalCase = string.Join("", parts.Select(p => char.ToUpperInvariant(p[0]) + p.Substring(1)));
        return Enum.Parse<T>(pascalCase, ignoreCase: true);
    }

    public static Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<T, string> GetConverter<T>() where T : struct, Enum
    {
        return new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<T, string>(
            v => ToSqlEnum(v),
            v => FromSqlEnum<T>(v)
        );
    }
}
```

Then use it in your DbContext:

```csharp
modelBuilder.Entity<Order>()
    .Property(o => o.Status)
    .HasConversion(EnumConverters.GetConverter<OrderStatus>());

modelBuilder.Entity<Order>()
    .Property(o => o.OrderType)
    .HasConversion(EnumConverters.GetConverter<OrderType>());

// ... and so on for all enum properties
```

### Manual Mapping for Special Cases

Some enums might need manual mapping:

```csharp
// For OrderType: SpecialOrder -> SPECIAL_ORDER
modelBuilder.Entity<Order>()
    .Property(o => o.OrderType)
    .HasConversion(
        v => v switch
        {
            OrderType.Service => "SERVICE",
            OrderType.Retail => "RETAIL",
            OrderType.Refurb => "REFURB",
            OrderType.SpecialOrder => "SPECIAL_ORDER",
            _ => throw new ArgumentOutOfRangeException(nameof(v))
        },
        v => v switch
        {
            "SERVICE" => OrderType.Service,
            "RETAIL" => OrderType.Retail,
            "REFURB" => OrderType.Refurb,
            "SPECIAL_ORDER" => OrderType.SpecialOrder,
            _ => throw new ArgumentOutOfRangeException(nameof(v))
        }
    );
```

## Recommendation

**For now, stick with VARCHAR** - it's simpler, more flexible, and works correctly with your current EF Core setup. The corrected schema I provided uses VARCHAR which matches your current EF Core configuration.

If you decide to use SQL ENUMs later, you can:
1. Update the SQL schema to use ENUM types
2. Add the value converters shown above to your DbContext
3. Test thoroughly to ensure data migration works correctly

## Current Status

Your corrected schema (`CORRECTED_SCHEMA.sql`) uses VARCHAR for all enum columns, which matches your current EF Core `.HasConversion<string>()` configuration. No changes needed to your C# code!

