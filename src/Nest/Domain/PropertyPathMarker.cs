﻿using System;
using System.Linq.Expressions;
using Elasticsearch.Net;

namespace Nest.Resolvers
{
	/// <summary>
	/// Represents a typed container for object paths "field.nested.property";
	/// </summary>
	public class PropertyPathMarker : IEquatable<PropertyPathMarker>
	{
		public string Name { get; set; }
		public Expression Type { get; set; }

		public double? Boost { get; set; }

		public static PropertyPathMarker Create(string path, double? boost = null)
		{
			PropertyPathMarker marker = path;
			marker.Boost = boost;
			return marker;
		}

		public static PropertyPathMarker Create(Expression path, double? boost = null)
		{
			PropertyPathMarker marker = path;
			marker.Boost = boost;
			return marker;
		}

		public static implicit operator PropertyPathMarker(string typeName)
		{
			return typeName == null ? null : new PropertyPathMarker { Name = typeName };
		}

		public static implicit operator PropertyPathMarker(Expression type)
		{
			return type == null ? null : new PropertyPathMarker { Type = type };
		}

		public override int GetHashCode()
		{
			if (this.Name != null)
				return this.Name.GetHashCode();
			return this.Type != null ? this.Type.GetHashCode() : 0;
		}

		bool IEquatable<PropertyPathMarker>.Equals(PropertyPathMarker other)
		{
			return Equals(other);
		}

		public override bool Equals(object obj)
		{
			var s = obj as string;
			if (!s.IsNullOrEmpty()) return this.EqualsString(s);
			var pp = obj as PropertyPathMarker;
			if (pp != null) return this.EqualsMarker(pp);

			return base.Equals(obj);
		}

		public bool EqualsMarker(PropertyPathMarker other)
		{
			return other != null && this.GetHashCode() == other.GetHashCode();
		}
		public bool EqualsString(string other)
		{
			return !other.IsNullOrEmpty() && other == this.Name;
		}
	}

	public static class PropertyPathMarkerExtensions
	{
		internal static bool IsConditionless(this PropertyPathMarker marker)
		{
			return marker == null || (marker.Name.IsNullOrEmpty() && marker.Type == null);
		}
	}

}