public static class Utils
{
  public static int getId(string s)
  {
    int ind = s.IndexOf("(") + 1;
    return int.Parse(s.Substring(ind, s.IndexOf(")") - ind));
  }
}
