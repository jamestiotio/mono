// StringTest.cs - NUnit Test Cases for the System.String class
//
// Jeffrey Stedfast <fejj@ximian.com>
// David Brandt <bucky@keystreams.com>
//
// (C) Ximian, Inc.  http://www.ximian.com
//

using NUnit.Framework;
using System;
using System.Globalization;

namespace MonoTests.System
{

public class StringTest : TestCase
{
	public StringTest() : base ("MonoTests.System.StringTest testcase") {}
	public StringTest (string name) : base (name) {}

	protected override void SetUp ()
	{
	}

	public static ITest Suite {
		get {
			return new TestSuite (typeof (StringTest));
		}
	}

	public void TestLength ()
	{
		string str = "test string";

		AssertEquals("wrong length", 11, str.Length);
	}

	public void TestCompare ()
	{
		string lesser = "abc";
		string medium = "abcd";
		string greater = "xyz";
		string caps = "ABC";

		AssertEquals(0, String.Compare(null, null));
		AssertEquals(1, String.Compare(lesser, null));

		Assert (String.Compare (lesser, greater) < 0);
		Assert (String.Compare (greater, lesser) > 0);
		Assert (String.Compare (lesser, lesser) == 0);
		Assert (String.Compare (lesser, medium) < 0);

		Assert (String.Compare (lesser, caps, true) == 0);
		Assert (String.Compare (lesser, caps, false) != 0);
		AssertEquals ("A01", String.Compare ("a", "b"), -1);
		AssertEquals ("A02", String.Compare ("b", "a"), 1);
		AssertEquals ("A03", String.Compare ("A", "a"), 1);
		AssertEquals ("A04", String.Compare ("a", "A"), -1);


		// TODO - test with CultureInfo

		string needle = "ab";
		string haystack = "abbcbacab";
		AssertEquals("basic substring check #1", 0, 
			     String.Compare(needle, 0, haystack, 0, 2));
		AssertEquals("basic substring check #2", -1,
			     String.Compare(needle, 0, haystack, 0, 3));
		AssertEquals("basic substring check #3", 0, 
			     String.Compare("ab", 0, "ab", 0, 2));
		AssertEquals("basic substring check #4", 0, 
			     String.Compare("ab", 0, "ab", 0, 3));
		AssertEquals("basic substring check #5", 0, 
			     String.Compare("abc", 0, "ab", 0, 2));
		AssertEquals("basic substring check #6", 1, 
			     String.Compare("abc", 0, "ab", 0, 5));
		AssertEquals("basic substring check #7", -1, 
			     String.Compare("ab", 0, "abc", 0, 5));

		for (int i = 1; i <= (haystack.Length - needle.Length); i++) {
			if (i != 7) {
				Assert("loop substring check #1/" + i, String.Compare(needle, 0, haystack, i, 2) != 0);
				Assert("loop substring check #2/" + i, String.Compare(needle, 0, haystack, i, 3) != 0);
			} else {
				AssertEquals("loop substring check #3/" + i, 0, String.Compare(needle, 0, haystack, i, 2));
				AssertEquals("loop substring check #4/" + i, 0, String.Compare(needle, 0, haystack, i, 3));
			}
		}

		needle = "AB";
		AssertEquals("basic substring check #8", 0, 
			     String.Compare(needle, 0, haystack, 0, 2, true));
		AssertEquals("basic substring check #9", 1, 
			     String.Compare(needle, 0, haystack, 0, 2, false));
		for (int i = 1; i <= (haystack.Length - needle.Length); i++) {
			if (i != 7) {
				Assert("loop substring check #5/" + i, String.Compare(needle, 0, haystack, i, 2, true) != 0);
				Assert("loop substring check #6/" + i, String.Compare(needle, 0, haystack, i, 2, false) != 0);
			} else {
				AssertEquals("loop substring check #7/" + i, 0, String.Compare(needle, 0, haystack, i, 2, true));
				AssertEquals("loop substring check #8/" + i, 1, String.Compare(needle, 0, haystack, i, 2, false));
			}
		}

		// TODO - extended format call with CultureInfo
	}

	public void TestCompareOrdinal ()
	{
		string lesser = "abc";
		string medium = "abcd";
		string greater = "xyz";

		AssertEquals(0, String.CompareOrdinal(null, null));
		AssertEquals(1, String.CompareOrdinal(lesser, null));

		Assert (String.CompareOrdinal (lesser, greater) < 0);
		Assert (String.CompareOrdinal (greater, lesser) > 0);
		Assert (String.CompareOrdinal (lesser, lesser) == 0);
		Assert (String.CompareOrdinal (lesser, medium) < 0);

		string needle = "ab";
		string haystack = "abbcbacab";
		AssertEquals("basic substring check", 0, 
			     String.CompareOrdinal(needle, 0, haystack, 0, 2));
		AssertEquals("basic substring miss", -1,
			     String.CompareOrdinal(needle, 0, haystack, 0, 3));
		for (int i = 1; i <= (haystack.Length - needle.Length); i++) {
			if (i != 7) {
				Assert("loop substring check " + i, String.CompareOrdinal(needle, 0, haystack, i, 2) != 0);
				Assert("loop substring check " + i, String.CompareOrdinal(needle, 0, haystack, i, 3) != 0);
			} else {
				AssertEquals("loop substring check " + i, 0, String.CompareOrdinal(needle, 0, haystack, i, 2));
				AssertEquals("loop substring check " + i, 0, String.CompareOrdinal(needle, 0, haystack, i, 3));
			}
		}
	}

	public void TestCompareTo ()
	{
		string lower = "abc";
		string greater = "xyz";
		string lesser = "abc";
		
		Assert (lower.CompareTo (greater) < 0);
		Assert (lower.CompareTo (lower) == 0);
		Assert (greater.CompareTo (lesser) > 0);
	}

	public void TestConcat ()
	{
		string string1 = "string1";
		string string2 = "string2";
		string concat = "string1string2";

		Assert (String.Concat (string1, string2) == concat);
	}

	public void TestCopy()
	{
		string s1 = "original";
		string s2 = String.Copy(s1);
		AssertEquals("String copy no good", s1, s2);

		bool errorThrown = false;
		try {
			string s = String.Copy(null);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null copy shouldn't be good", errorThrown);
	}

	public void TestCopyTo()
	{
		string s1 = "original";

		bool errorThrown = false;
		try {
			s1.CopyTo(0, (char[])null, 0, s1.Length);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null CopyTo shouldn't be good", errorThrown);
		
		char[] c1 = new char[s1.Length];
		string s2 = new String(c1);
		Assert("char string not bad to start", !s1.Equals(s2));
		for (int i = 0; i < s1.Length; i++) {
			s1.CopyTo(i, c1, i, 1);
		}
		s2 = new String(c1);
		AssertEquals("char-by-char copy bad", s1, s2);
	}

	public void TestEndsWith()
	{
		string s1 = "original";
		
		bool errorThrown = false;
		try {
			bool huh = s1.EndsWith(null);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null EndsWith shouldn't be good", errorThrown);

		Assert("should match", s1.EndsWith("l"));
		Assert("should match 2", s1.EndsWith("inal"));
		Assert("should fail", !s1.EndsWith("ina"));
	}

	public void TestEquals()
	{
		string s1 = "original";
		string yes = "original";
		object y = yes;
		string no = "copy";

		Assert("No match for null", !s1.Equals(null));
		Assert("Should match object", s1.Equals(y));
		Assert("Should match", s1.Equals(yes));
		Assert("Shouldn't match", !s1.Equals(no));

		Assert("Static nulls should match", String.Equals(null, null));
		Assert("Should match", String.Equals(s1, yes));
		Assert("Shouldn't match", !String.Equals(s1, no));
	}

	public void TestFormat ()
	{
		AssertEquals ("Empty format string.", "", String.Format ("", 0));
		AssertEquals ("Single argument.", "100", String.Format ("{0}", 100));
		AssertEquals ("Single argument, right justified.", "X   37X", String.Format ("X{0,5}X", 37));
		AssertEquals ("Single argument, left justified.", "X37   X", String.Format ("X{0,-5}X", 37));
		AssertEquals ("Two arguments.", "The 3 wise men.", String.Format ("The {0} wise {1}.", 3, "men"));
		AssertEquals ("Three arguments.", "do re me fa so.", String.Format ("{0} re {1} fa {2}.", "do", "me", "so"));
		AssertEquals ("Formatted argument.", "###00c0ffee#", String.Format ("###{0:x8}#", 0xc0ffee));
		AssertEquals ("Formatted argument, right justified.", "#  033#", String.Format ("#{0,5:x3}#", 0x33));
		AssertEquals ("Formatted argument, left justified.", "#033  #", String.Format ("#{0,-5:x3}#", 0x33));
		AssertEquals ("Escaped bracket", "typedef struct _MonoObject { ... } MonoObject;", String.Format ("typedef struct _{0} {{ ... }} MonoObject;", "MonoObject"));


		// TODO test format exceptions

		// TODO test argument null exceptions
		//   This should work, but it doesn't currently.
		//   I think I broke the spec...
		//bool errorThrown = false;
		//try {
		//string s = String.Format(null, " ");
		//} catch (ArgumentNullException) {
		//errorThrown = true;
		//}
		//Assert("error not thrown 1", errorThrown);
		//errorThrown = false;
		//try {
		//string s = String.Format(null, " ", " ");
		//} catch (ArgumentNullException) {
		//errorThrown = true;
		//}
		//Assert("error not thrown 2", errorThrown);
		//errorThrown = false;
		//try {
		//string s = String.Format(" ", null);
		//} catch (ArgumentNullException) {
		//errorThrown = true;
		//}
		//Assert("error not thrown 3", errorThrown);
	}

	public void TestGetEnumerator()
	{
		string s1 = "original";
		char[] c1 = new char[s1.Length];
		string s2 = new String(c1);
		Assert("pre-enumerated string should not match", !s1.Equals(s2));
		CharEnumerator en = s1.GetEnumerator();
		AssertNotNull("null enumerator", en);
		
		for (int i = 0; i < s1.Length; i++) {
			en.MoveNext();
			c1[i] = en.Current;
		}
		s2 = new String(c1);
		AssertEquals("enumerated string should match", s1, s2);
	}

	public void TestGetHashCode()
	{
		string s1 = "original";
		// TODO - weak test, currently.  Just verifies determinicity.
		AssertEquals("same string, same hash code", 
			     s1.GetHashCode(), s1.GetHashCode());
	}

	public void TestGetType() {
		string s1 = "original";
		AssertEquals("String type", "System.String", s1.GetType().ToString());
	}

	public void TestGetTypeCode() {
		string s1 = "original";
		Assert(s1.GetTypeCode().Equals(TypeCode.String));
	}

	public void TestIndexOf() {
		string s1 = "original";

		bool errorThrown = false;
		try {
			int i = s1.IndexOf('q', s1.Length + 1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("out of range error for char", errorThrown);

		errorThrown = false;
		try {
			int i = s1.IndexOf('q', s1.Length + 1, 1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("out of range error for char", errorThrown);

		errorThrown = false;
		try {
			int i = s1.IndexOf("huh", s1.Length + 1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("out of range for string", errorThrown);

		errorThrown = false;
		try {
			int i = s1.IndexOf("huh", s1.Length + 1, 3);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("out of range for string", errorThrown);

		errorThrown = false;
		try {
			int i = s1.IndexOf(null);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null string error", errorThrown);

		errorThrown = false;
		try {
			int i = s1.IndexOf(null, 0);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null string error", errorThrown);

		errorThrown = false;
		try {
			int i = s1.IndexOf(null, 0, 1);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null string error", errorThrown);

		AssertEquals("basic char index", 1, s1.IndexOf('r'));
		AssertEquals("basic char index", 2, s1.IndexOf('i'));
		AssertEquals("basic char index - no", -1, s1.IndexOf('q'));
		
		AssertEquals("basic string index", 1, s1.IndexOf("rig"));
		AssertEquals("basic string index", 2, s1.IndexOf("i"));
		AssertEquals("basic string index - no", -1, s1.IndexOf("rag"));

		AssertEquals("stepped char index", 1, s1.IndexOf('r', 1));
		AssertEquals("stepped char index", 2, s1.IndexOf('i', 1));
		AssertEquals("stepped char index", 4, s1.IndexOf('i', 3));
		AssertEquals("stepped char index", -1, s1.IndexOf('i', 5));

		AssertEquals("stepped limited char index", 
			     1, s1.IndexOf('r', 1, 1));
		AssertEquals("stepped limited char index", 
			     -1, s1.IndexOf('r', 0, 1));
		AssertEquals("stepped limited char index", 
			     2, s1.IndexOf('i', 1, 3));
		AssertEquals("stepped limited char index", 
			     4, s1.IndexOf('i', 3, 3));
		AssertEquals("stepped limited char index", 
			     -1, s1.IndexOf('i', 5, 3));

		s1 = "original original";
		AssertEquals("stepped string index",
			     0, s1.IndexOf("original", 0));
		AssertEquals("stepped string index", 
			     9, s1.IndexOf("original", 1));
		AssertEquals("stepped string index", 
			     -1, s1.IndexOf("original", 10));
		AssertEquals("stepped limited string index",
			     1, s1.IndexOf("rig", 0, 5));
		AssertEquals("stepped limited string index",
			     -1, s1.IndexOf("rig", 0, 3));
		AssertEquals("stepped limited string index",
			     10, s1.IndexOf("rig", 2, 15));
		AssertEquals("stepped limited string index",
			     -1, s1.IndexOf("rig", 2, 3));
	}

	public void TestIndexOfAny() {
		string s1 = "abcdefghijklm";

		bool errorThrown = false;
		try {
			int i = s1.IndexOfAny(null);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null char[] error", errorThrown);

		errorThrown = false;
		try {
			int i = s1.IndexOfAny(null, 0);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null char[] error", errorThrown);

		errorThrown = false;
		try {
			int i = s1.IndexOfAny(null, 0, 1);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null char[] error", errorThrown);

		char[] c1 = {'a', 'e', 'i', 'o', 'u'};
		AssertEquals("first vowel", 0, s1.IndexOfAny(c1));
		AssertEquals("second vowel", 4, s1.IndexOfAny(c1, 1));
		AssertEquals("out of vowels", -1, s1.IndexOfAny(c1, 9));
		AssertEquals("second vowel in range", 
			     4, s1.IndexOfAny(c1, 1, 4));
		AssertEquals("second vowel out of range", 
			     -1, s1.IndexOfAny(c1, 1, 3));

		errorThrown = false;
		try {
			int i = s1.IndexOfAny(c1, s1.Length+1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("Out of range error", errorThrown);

		errorThrown = false;
		try {
			int i = s1.IndexOfAny(c1, s1.Length+1, 1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("Out of range error", errorThrown);
	}

	public void TestInsert() {
		string s1 = "original";
		
		bool errorThrown = false;
		try {
			string result = s1.Insert(0, null);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("Null arg error", errorThrown);

		errorThrown = false;
		try {
			string result = s1.Insert(s1.Length+1, "Hi!");
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("Out of range error", errorThrown);

		AssertEquals("front insert", 
			     "Hi!original", s1.Insert(0, "Hi!"));
		AssertEquals("back insert", 
			     "originalHi!", s1.Insert(s1.Length, "Hi!"));
		AssertEquals("middle insert", 
			     "origHi!inal", s1.Insert(4, "Hi!"));
	}

	public void TestIntern() {
		bool errorThrown = false;
		try {
			string s = String.Intern(null);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null arg error", errorThrown);

		string s1 = "original";
		AssertEquals("One string's reps are both the same",
			     String.Intern(s1), String.Intern(s1));

		string s2 = "originally";
		Assert("Different strings, different reps",
		       String.Intern(s1) != String.Intern(s2));
	}

	public void TestIsInterned() {
		bool errorThrown = false;
		try {
			string s = String.IsInterned(null);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null arg error", errorThrown);

		// FIXME - it seems like this should work, but no.
		//   I don't know how it's possible to get a null
		//   returned from IsInterned.
		//Assert("no intern for regular string", 
		//String.IsInterned("original") == null);

		string s1 = "original";
		AssertEquals("is interned", s1, String.IsInterned(s1));
	}

	public void TestJoin() {
		bool errorThrown = false;
		try {
			string s = String.Join(" ", null);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null arg error", errorThrown);

		string[] chunks = {"this", "is", "a", "test"};
		AssertEquals("Basic join", "this is a test",
			     String.Join(" ", chunks));
		AssertEquals("Basic join", "this.is.a.test",
			     String.Join(".", chunks));

		AssertEquals("Subset join", "is a",
			     String.Join(" ", chunks, 1, 2));
		AssertEquals("Subset join", "is.a",
			     String.Join(".", chunks, 1, 2));
		AssertEquals("Subset join", "is a test",
			     String.Join(" ", chunks, 1, 3));

		errorThrown = false;
		try {
			string s = String.Join(" ", chunks, 2, 3);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("out of range error", errorThrown);
	}

	public void TestLastIndexOf() {
		string s1 = "original";

		bool errorThrown = false;
		try {
			int i = s1.LastIndexOf('q', -1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("out of range error for char", errorThrown);

		errorThrown = false;
		try {
			int i = s1.LastIndexOf('q', -1, 1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("out of range error for char", errorThrown);

		errorThrown = false;
		try {
			int i = s1.LastIndexOf("huh", s1.Length + 1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("out of range for string", errorThrown);

		errorThrown = false;
		try {
			int i = s1.LastIndexOf("huh", s1.Length + 1, 3);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("out of range for string", errorThrown);

		errorThrown = false;
		try {
			int i = s1.LastIndexOf(null);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null string error", errorThrown);

		errorThrown = false;
		try {
			int i = s1.LastIndexOf(null, 0);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null string error", errorThrown);

		errorThrown = false;
		try {
			int i = s1.LastIndexOf(null, 0, 1);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null string error", errorThrown);

		AssertEquals("basic char index", 1, s1.LastIndexOf('r'));
		AssertEquals("basic char index", 4, s1.LastIndexOf('i'));
		AssertEquals("basic char index - no", -1, s1.LastIndexOf('q'));
		
		AssertEquals("basic string index", 1, s1.LastIndexOf("rig"));
		AssertEquals("basic string index", 4, s1.LastIndexOf("i"));
		AssertEquals("basic string index - no", -1, 
			     s1.LastIndexOf("rag"));



		AssertEquals("stepped char index", 1, 
			     s1.LastIndexOf('r', s1.Length-1));
		AssertEquals("stepped char index", 4, 
			     s1.LastIndexOf('i', s1.Length-1));
		AssertEquals("stepped char index", 2, 
			     s1.LastIndexOf('i', 3));
		AssertEquals("stepped char index", -1, 
			     s1.LastIndexOf('i', 1));

		AssertEquals("stepped limited char index", 
			     1, s1.LastIndexOf('r', 1, 1));
		AssertEquals("stepped limited char index", 
			     -1, s1.LastIndexOf('r', 0, 1));
		AssertEquals("stepped limited char index", 
			     4, s1.LastIndexOf('i', 6, 3));
		AssertEquals("stepped limited char index", 
			     2, s1.LastIndexOf('i', 3, 3));
		AssertEquals("stepped limited char index", 
			     -1, s1.LastIndexOf('i', 1, 2));

		s1 = "original original";
		AssertEquals("stepped string index",
			     9, s1.LastIndexOf("original", s1.Length));
		AssertEquals("stepped string index", 
			     0, s1.LastIndexOf("original", s1.Length-2));
		AssertEquals("stepped string index", 
			     -1, s1.LastIndexOf("original", s1.Length-11));
		AssertEquals("stepped string index",
			     -1, s1.LastIndexOf("translator", 2));
		AssertEquals("stepped limited string index",
			     10, s1.LastIndexOf("rig", s1.Length-1, 10));
		AssertEquals("stepped limited string index",
			     -1, s1.LastIndexOf("rig", s1.Length, 3));
		AssertEquals("stepped limited string index",
			     10, s1.LastIndexOf("rig", s1.Length-2, 15));
		AssertEquals("stepped limited string index",
			     -1, s1.LastIndexOf("rig", s1.Length-2, 3));
	}

	public void TestLastIndexOfAny() {
		string s1 = ".bcdefghijklm";

		bool errorThrown = false;
		try {
			int i = s1.LastIndexOfAny(null);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null char[] error", errorThrown);

		errorThrown = false;
		try {
			int i = s1.LastIndexOfAny(null, s1.Length);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null char[] error", errorThrown);

		errorThrown = false;
		try {
			int i = s1.LastIndexOfAny(null, s1.Length, 1);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null char[] error", errorThrown);

		char[] c1 = {'a', 'e', 'i', 'o', 'u'};
		AssertEquals("first vowel", 8, s1.LastIndexOfAny(c1));
		AssertEquals("second vowel", 4, s1.LastIndexOfAny(c1, 7));
		AssertEquals("out of vowels", -1, s1.LastIndexOfAny(c1, 3));
		AssertEquals("second vowel in range", 
			     4, s1.LastIndexOfAny(c1, s1.Length-6, 4));
		AssertEquals("second vowel out of range", 
			     -1, s1.LastIndexOfAny(c1, s1.Length-6, 3));

		errorThrown = false;
		try {
			int i = s1.LastIndexOfAny(c1, -1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("Out of range error", errorThrown);

		errorThrown = false;
		try {
			int i = s1.LastIndexOfAny(c1, -1, 1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("Out of range error", errorThrown);
	}

	public void TestPadLeft() {
		string s1 = "Hi!";

		bool errorThrown = false;
		try {
			string s = s1.PadLeft(-1);
		} catch (ArgumentException) {
			errorThrown = true;
		}
		Assert("Bad argument error", errorThrown);

		AssertEquals("Too little padding",
			     s1, s1.PadLeft(s1.Length-1));
		AssertEquals("Some padding",
			     "  Hi!", s1.PadLeft(5));
	}

	public void TestPadRight() {
		string s1 = "Hi!";

		bool errorThrown = false;
		try {
			string s = s1.PadRight(-1);
		} catch (ArgumentException) {
			errorThrown = true;
		}
		Assert("Bad argument error", errorThrown);

		AssertEquals("Too little padding",
			     s1, s1.PadRight(s1.Length-1));
		AssertEquals("Some padding",
			     "Hi!  ", s1.PadRight(5));
	}

	public void TestRemove() {
		string s1 = "original";

		bool errorThrown = false;
		try {
			s1.Remove(-1,1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("out of range error", errorThrown);
		errorThrown = false;
		try {
			s1.Remove(1,-1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("out of range error", errorThrown);
		errorThrown = false;
		try {
			s1.Remove(s1.Length,s1.Length);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("out of range error", errorThrown);

		AssertEquals("basic remove", "oinal",
			     s1.Remove(1, 3));
	}

	public void TestReplace() {
		string s1 = "original";

		AssertEquals("non-hit char", s1, s1.Replace('q','s'));
		AssertEquals("single char", "oxiginal", s1.Replace('r', 'x'));
		AssertEquals("double char", "orxgxnal", s1.Replace('i', 'x'));

		bool errorThrown = false;
		try {
			string s = s1.Replace(null, "feh");
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("should get null arg exception", errorThrown);

		AssertEquals("replace as remove", "ornal", 
			     s1.Replace("igi", null));
		AssertEquals("non-hit string", s1, s1.Replace("spam", "eggs"));
		AssertEquals("single string", "orirumal", 
			     s1.Replace("gin", "rum"));
		AssertEquals("double string", "oreigeinal", 
			     s1.Replace("i", "ei"));
	}

	public void TestSplit() {
		string s1 = "abcdefghijklm";
		char[] c1 = {'q', 'r'};
		AssertEquals("No splitters", s1, (s1.Split(c1))[0]);

		char[] c2 = {'a', 'e', 'i', 'o', 'u'};
		string[] chunks = s1.Split(c2);
		AssertEquals("First chunk", "", chunks[0]);
		AssertEquals("Second chunk", "bcd", chunks[1]);
		AssertEquals("Third chunk", "fgh", chunks[2]);
		AssertEquals("Fourth chunk", "jklm", chunks[3]);

		{
			bool errorThrown = false;
			try {
				chunks = s1.Split(c2, -1);
			} catch (ArgumentOutOfRangeException) {
				errorThrown = true;
			}
			Assert("Split out of range", errorThrown);
		}

		chunks = s1.Split(c2, 2);
		AssertEquals("Limited chunk", 2, chunks.Length);
		AssertEquals("First limited chunk", "", chunks[0]);
		AssertEquals("Second limited chunk", "bcdefghijklm", chunks[1]);

		chunks = s1.Split(c2, 1);
		AssertEquals("Single split", 1, chunks.Length);
		AssertEquals("Single chunk", s1, chunks[0]);

		chunks = s1.Split(c2, 0);
		AssertEquals("Zero split", 0, chunks.Length);
	}

	public void TestStartsWith() {
		string s1 = "original";
		
		bool errorThrown = false;
		try {
			bool huh = s1.StartsWith(null);
		} catch (ArgumentNullException) {
			errorThrown = true;
		}
		Assert("null StartsWith shouldn't be good", errorThrown);

		Assert("should match", s1.StartsWith("o"));
		Assert("should match 2", s1.StartsWith("orig"));
		Assert("should fail", !s1.StartsWith("rig"));
	}

	public void TestSubstring() {
		string s1 = "original";

		bool errorThrown = false;
		try {
			string s = s1.Substring(s1.Length+1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("error not thrown", errorThrown);
		errorThrown = false;
		try {
			string s = s1.Substring(-1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("error not thrown", errorThrown);

		errorThrown = false;
		try {
			string s = s1.Substring(1, -1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("error not thrown", errorThrown);
		errorThrown = false;
		try {
			string s = s1.Substring(-1, 1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("error not thrown", errorThrown);
		errorThrown = false;
		try {
			string s = s1.Substring(s1.Length, 1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("error not thrown", errorThrown);
		errorThrown = false;
		try {
			string s = s1.Substring(1, s1.Length);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("error not thrown", errorThrown);

		AssertEquals("basic substring", "inal",
			     s1.Substring(4));
		AssertEquals("midstring", "igin",
			     s1.Substring(2, 4));
	}

	public void TestToCharArray() {
		string s1 = "original";
		char[] c1 = s1.ToCharArray();
		AssertEquals("right array size", s1.Length, c1.Length);
		AssertEquals("basic char array", s1,
			     new String(c1));
		
		bool errorThrown = false;
		try {
			s1.ToCharArray(s1.Length, 1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("error not thrown", errorThrown);
		errorThrown = false;
		try {
			s1.ToCharArray(1, s1.Length);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("error not thrown", errorThrown);
		errorThrown = false;
		try {
			s1.ToCharArray(-1, 1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("error not thrown", errorThrown);
		errorThrown = false;
		try {
			s1.ToCharArray(1, -1);
		} catch (ArgumentOutOfRangeException) {
			errorThrown = true;
		}
		Assert("error not thrown", errorThrown);

		c1 = s1.ToCharArray(0, 3);
		AssertEquals("Starting char array", "ori", new String(c1));
	}

	public void TestToLower() {
		string s1 = "OrIgInAl";
		AssertEquals("lowercase failed", "original", s1.ToLower());

		// TODO - Again, with CultureInfo
	}

	public void TestToString() {
		string s1 = "original";
		AssertEquals("ToString failed!", s1, s1.ToString());
	}

	public void TestToUpper() {
		string s1 = "OrIgInAl";
		AssertEquals("uppercase failed", "ORIGINAL", s1.ToUpper());

		// TODO - Again, with CultureInfo
	}

	public void TestTrim() {
		string s1 = "  original\t\n";
		AssertEquals("basic trim failed", "original", s1.Trim());
		AssertEquals("basic trim failed", "original", s1.Trim(null));

		s1 = "original";
		AssertEquals("basic trim failed", "original", s1.Trim());
		AssertEquals("basic trim failed", "original", s1.Trim(null));

		s1 = "   \t \n  ";
		AssertEquals("empty trim failed", "", s1.Trim());
		AssertEquals("empty trim failed", "", s1.Trim(null));

		s1 = "aaaoriginalbbb";
		char[] delims = {'a', 'b'};
		AssertEquals("custom trim failed", 
			     "original", s1.Trim(delims));
	}

	public void TestTrimEnd() {
		string s1 = "  original\t\n";
		AssertEquals("basic TrimEnd failed", 
			     "  original", s1.TrimEnd(null));

		s1 = "  original";
		AssertEquals("basic TrimEnd failed", 
			     "  original", s1.TrimEnd(null));

		s1 = "  \t  \n  \n    ";
		AssertEquals("empty TrimEnd failed", 
			     "", s1.TrimEnd(null));

		s1 = "aaaoriginalbbb";
		char[] delims = {'a', 'b'};
		AssertEquals("custom TrimEnd failed", 
			     "aaaoriginal", s1.TrimEnd(delims));
	}

	public void TestTrimStart() {
		string s1 = "  original\t\n";
		AssertEquals("basic TrimStart failed", 
			     "original\t\n", s1.TrimStart(null));

		s1 = "original\t\n";
		AssertEquals("basic TrimStart failed", 
			     "original\t\n", s1.TrimStart(null));

		s1 = "    \t \n \n  ";
		AssertEquals("empty TrimStart failed", 
			     "", s1.TrimStart(null));

		s1 = "aaaoriginalbbb";
		char[] delims = {'a', 'b'};
		AssertEquals("custom TrimStart failed", 
			     "originalbbb", s1.TrimStart(delims));
	}

}

}
