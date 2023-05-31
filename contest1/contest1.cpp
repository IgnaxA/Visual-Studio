#include <iostream>
#include <vector>
#include <algorithm>
#include <fstream>
#include <string>
#include <iomanip>
#include <map>
#define ll long long
using namespace std;

int main()
{
	cin.tie(0); 
	ios_base::sync_with_stdio(false);
	
	ll q;
	vector<ll> delen(6);

	cin >> q;
	for (int i = 0; i < 6; ++i) cin >> delen[i];

	int lBor = 0, rBor = 100, mid = 50;
	while (true)
	{
		ll sum = delen[0], stepSum = 1;
		bool isOk = true;
		for (int i = 1; i < 6; ++i)
		{
			stepSum *= mid;
			sum += (delen[i] * stepSum);
		}

		if (sum > q)
		{
			rBor = mid;
			mid = (rBor + lBor) / 2;
		}
		else if (sum == q)
		{
			cout << mid;
			break;
		}
		else
		{
			lBor = mid;
			mid = (rBor + lBor) / 2;
		}

		if (lBor == 0 && rBor == 1)
		{
			cout << 0;
			break;
		}
		if (lBor == 99 && rBor == 100)
		{
			cout << -1;
			break;
		}
	}
}