using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Cubic{
		public int a;
		public int b;
		public int c;
		public Cubic(int a,int b,int c){
			this.a = a;
			this.b = b;
			this.c = c;
		}
	}

	public class EmptyClass
	{


		public static void Main2() {
			string line;
			line = System.Console.ReadLine ();
			int n = int.Parse(line);
			if(n==0){
				System.Console.WriteLine(0);
				return;
			}
			List<Cubic> ss = new List<Cubic> ();
			//ss.Add(new Cubic(0,0,0));
			for(int i=0;i<n;i++){
				line = System.Console.ReadLine ();
				string[] tokens = line.Split();
				int a = int.Parse(tokens[0]);
				int b = int.Parse(tokens[1]);
				int c = int.Parse(tokens[2]);
				ss.Add(new Cubic(a,b,c));

			}

			ss.Sort (delegate(Cubic x, Cubic y) {
				if(x.b*x.c<y.b*y.c)return 1;
				else if(x.b*x.c>y.b*y.c)return -1;
				else return 0;
			});

			int[] dp = new int[n];
			dp[0] = ss[0].a;
			for(int i=1;i<dp.Length;i++){
				int lmax = 0;
				for(int j=i-1;j>=0;j--){
					if((ss[j].b>ss[i].b&&ss[j].c>ss[i].c)||(ss[j].b>ss[i].c&&ss[j].c>ss[i].b)){
						if(dp[j] + ss[i].a > lmax){
							lmax = dp[j] + ss[i].a;
						}
					}
				}
				dp[i] = lmax;
			}
			int max = 0;
			for(int i=0;i<dp.Length;i++){
				if(dp[i]>max){
					max = dp[i];
				} 
			}
			System.Console.WriteLine(max);
		}


		public static void Main ()
		{
			string line;
			List<Cubic> ss = new List<Cubic> ();
			ss.Sort (delegate(Cubic x, Cubic y) {
				if(x.a*x.b<y.a*y.b)return -1;
				else if(x.a*x.b>y.a*y.b)return 1;
				else return 0;
			});



			line = System.Console.ReadLine ();
			int m = int.Parse (line);
			line = System.Console.ReadLine ();
			int n = int.Parse (line);
			line = System.Console.ReadLine ();
			string[] tokens = line.Split ();

			for (int i = 0; i < tokens.Length; i++) {
				data.Add (int.Parse (tokens [i]));
			}
			System.Console.WriteLine (res);
		}

		static List<int> data = new List<int> ();
		static bool found = false;
		static bool res = false;

		public static void func (int target, int idx, List<int> choose)
		{
			if (res)
				return;
			if (idx >= data.Count)
				return;
			if (target > data [idx]) {
				choose.Add (idx);
				func (target - data [idx], idx + 1,choose);
				choose.Remove (idx);
			} else if (target == data [idx]) {
				int subsum = 0;
				for (int i = 0; i < choose.Count; i++) {
					subsum += data [choose [i]];
				}
				if (subsum % 2 == 0) {
					found = false;
					func2 (subsum / 2, 0, choose, new List<int> ());
					List<int> unchoose = new List<int> ();
					for (int i = 0; i < data.Count; i++) {
						if (!choose.Contains (i)) {
							unchoose.Add (i);
						}
					}
					if (found == true) {
						found = false;
						func2 (subsum / 2, 0, unchoose, new List<int> ());
						if (found == true) {
							res = true;
						}
					}

				}

			}
			func (target, idx + 1,choose);

		}

		public static void func2 (int target, int idx, List<int> toChoose, List<int> choose)
		{
			if (found)
				return;
			if (idx >= toChoose.Count)
				return;
			if (target > data [toChoose [idx]]) {
				choose.Add (toChoose [idx]);
				func2 (target - data [toChoose [idx]], idx + 1,toChoose,choose);
				choose.Add (toChoose [idx]);
			} else if (target == data [toChoose [idx]]) {
				found = true;
			}
			func2 (target, idx + 1,toChoose,choose);
		}
	}
}

