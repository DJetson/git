using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WindowsGameOne.Extensions
{
    public static class ExtensionMethods
    {
        public static string ElementValueNull(this XElement element)
        {
            if (element != null)
                return element.Value;

            return "";
        }

        public static string AttributeValueNull(this XElement element, string attributeName)
        {
            if (element == null)
                return "";
            else
            {
                XAttribute attr = element.Attribute(attributeName);
                return attr == null ? "" : attr.Value;
            }
        }

        //public static float ParseTokens(this string value)
        //{
        //    String Tokens = value;
        //    if (Tokens.StartsWith("Viewport."))
        //    {
        //        if (Tokens.Contains("Width"))
        //            Tokens = Tokens.Replace("Viewport.Width", Game1.ViewportSize.X.ToString());
        //        if (Tokens.Contains("Height"))
        //            Tokens = Tokens.Replace("Viewport.Height", Game1.ViewportSize.Y.ToString());
        //        if (Tokens.Contains("-"))
        //        {
        //            String[] values = Tokens.Split(new char[] {'-'});
        //            return float.Parse(values[0].Trim()) - float.Parse(values[1].Trim());
        //        }
        //    }
        //    else if (Tokens.StartsWith("Camera."))
        //    {
        //        if (Tokens.Contains("Width"))
        //            Tokens = Tokens.Replace("Camera.Width", Game1.Camera.ViewportSize.X.ToString());
        //        if (Tokens.Contains("Height"))
        //            Tokens = Tokens.Replace("Camera.Height", Game1.Camera.ViewportSize.Y.ToString());
        //        if (Tokens.Contains("Position.X"))
        //            Tokens = Tokens.Replace("Camera.Position.X", Game1.Camera.Position.X.ToString());
        //        if (Tokens.Contains("Position.Y"))
        //            Tokens = Tokens.Replace("Camera.Position.Y", Game1.Camera.Position.Y.ToString());
        //        if (Tokens.Contains("-"))
        //        {
        //            String[] values = Tokens.Split(new char[] { '-' });
        //            return float.Parse(values[0].Trim()) - float.Parse(values[1].Trim());
        //        }
        //    }
        //    return float.Parse(Tokens);
        //}

        public static T ParseTokens<T>(this string value)
        {
            Type t = typeof(T);
            T ret;
            String Tokens = value;
            if (Tokens.StartsWith("Viewport."))
            {
                if (Tokens.Contains("Width"))
                    Tokens = Tokens.Replace("Viewport.Width", Game1.ViewportSize.X.ToString());
                if (Tokens.Contains("Height"))
                    Tokens = Tokens.Replace("Viewport.Height", Game1.ViewportSize.Y.ToString());
                if (Tokens.Contains("-"))
                {
                    String[] values = Tokens.Split(new char[] { '-' });
                    object valA = (T)t.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { values[0].Trim() });
                    object valB = (T)t.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { values[1].Trim() });
                    return (T)(object)((float)valA - (float)valB);
                }
            }
            else if (Tokens.StartsWith("Level."))
            {
                if (Tokens.Contains("Width"))
                    Tokens = Tokens.Replace("Level.Width", Game1.CurrentLevel.Size.X.ToString());
                if (Tokens.Contains("Height"))
                    Tokens = Tokens.Replace("Level.Height", Game1.CurrentLevel.Size.Y.ToString());
                if (Tokens.Contains("-"))
                {
                    String[] values = Tokens.Split(new char[] { '-' });
                    object valA = (T)t.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { values[0].Trim() });
                    object valB = (T)t.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { values[1].Trim() });
                    //if (valA is float && valB is float)
                    return (T)(object)((float)valA - (float)valB);
                }
            }
            else if (Tokens.StartsWith("Camera."))
            {
                if (Tokens.Contains("Width"))
                    Tokens = Tokens.Replace("Camera.Width", Game1.Camera.ViewportSize.X.ToString());
                if (Tokens.Contains("Height"))
                    Tokens = Tokens.Replace("Camera.Height", Game1.Camera.ViewportSize.Y.ToString());
                if (Tokens.Contains("Position.X"))
                    Tokens = Tokens.Replace("Camera.Position.X", Game1.Camera.Position.X.ToString());
                if (Tokens.Contains("Position.Y"))
                    Tokens = Tokens.Replace("Camera.Position.Y", Game1.Camera.Position.Y.ToString());
                if (Tokens.Contains("-"))
                {
                    String[] values = Tokens.Split(new char[] { '-' });
                    object valA = (T)t.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { values[0].Trim() });
                    object valB = (T)t.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { values[1].Trim() });

                    //if (valA is float && valB is float)
                    return (T)(object)((float)valA - (float)valB);

                    //return valA - valB; // float.Parse(values[0].Trim()) - float.Parse(values[1].Trim());
                }
            }
            ret = (T)t.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { Tokens });
            return ret;// float.Parse(Tokens);
        }


        public static float ConvertToFloat(this string value)
        {

            try
            {
                if (String.IsNullOrEmpty(value))
                    return 0;
                return ParseTokens<float>(value);
                //else if (value.StartsWith("Viewport."))
                //{
                //    string token = null;
                //    if (value.Contains("Height"))
                //        token = value.Replace("Viewport.Height",Game1.ViewportSize.Y.ToString());
                //    else if (value.Contains("Width"))
                //        token = value.Replace("Viewport.Width", Game1.ViewportSize.Y.ToString());
                //    return float.Parse((string)(token ?? "0")) ;
                //}

                ////else
                //    return float.Parse(value);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("ConvertToFloat() failed. The value \"{0}\" could not be converted.", value), ex);
            }
        }

        public static int ConvertToInt(this string value)
        {
            try
            {
                if (String.IsNullOrEmpty(value))
                    return 0;
                return ParseTokens<int>(value);
                //if (String.IsNullOrEmpty(value))
                //    return 0;
                //else if (value.StartsWith("Viewport."))
                //{
                //    string token = null;
                //    if (value.EndsWith("Height"))
                //        token = value.Replace("Viewport.Height", Game1.ViewportSize.Y.ToString());
                //    else if (value.EndsWith("Width"))
                //        token = value.Replace("Viewport.Width", Game1.ViewportSize.Y.ToString());
                //    return int.Parse((string)(token ?? "0"));
                //}
                ////else
                //    return int.Parse(value);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("ConvertToInt() failed. The value \"{0}\" could not be converted", value), ex);
            }
        }

        public static bool ConvertToBoolean(this string value)
        {
            try
            {
                if (String.IsNullOrEmpty(value))
                    return true;
                else
                    return bool.Parse(value);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("ConvertToBoolean() failed. The value \"{0}\" could not be converted", value), ex);
            }
        }

        /// <summary>
        /// Scale a color by the provided factor
        /// </summary>
        /// <param name="value">The initial color</param>
        /// <param name="ScaleBy">The scale factor. A value greater than 0.0</param>
        /// <returns>The new scaled color</returns>
        public static Color ScaleColor(this Color value, double ScaleBy)
        {
            //Clamp the Scale Factor
            //if(ScaleBy > 1.0)
            //    ScaleBy = 1;
            //else 
            if (ScaleBy < 0)
                ScaleBy = 0;

            Color NewColor = new Color((int)(((value.R / 255) * ScaleBy) * 255), (int)(((value.G / 255) * ScaleBy) * 255), (int)(((value.B / 255) * ScaleBy) * 255), value.A);

            return NewColor;
        }
    }
}
