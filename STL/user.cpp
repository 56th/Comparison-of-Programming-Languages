#include <iostream>
#include <fstream>
#include <vector>
#include <map>
#include <set>
#include <string>
#include <algorithm>
#include <ctime> // çàìåðèòü âðåìÿ
#include <cctype> // isalnum

using namespace std; // ïðîãðàììà êîðîòåíüêàÿ, òàê ÷òî ìîæíî è ðàññëàáèòüñÿ

// ñåãìåíò äàííûõ

vector<string> book; // òåêñò êíèãè 
string word; // ñàìîå ïîïóëÿðíîå ñëîâî
set<string> words2ignore; // èãíîðèðóåìûå ñëîâà (ïðåäëîãè, àðòèêëè è ò.ä.)
unsigned N = 15; // êîëè÷åñòâî âûâîäèìûõ ñî÷åòàíèé ñëîâ


void getBook() { // ñ÷èòûâàåì òåêñò êíèæêè èç ñòàíäàðòíîãî ââîäà â êîíòåéíåð
	while (cin >> word) {
		transform(word.begin(), word.end(), word.begin(), ::tolower); // ïðîïèñíûå áóêâû ê ñòðî÷íûì
		word.erase(remove_if(word.begin(), word.end(), [](char c){return !isalnum(c);}), word.end()); // èçáàâëÿåìñÿ îò ëèøíèõ ñèìâîëîâ â ñëîâå
		if (word.length()) book.push_back(word);
	}
}

void getWord() { // íàõîäèì ñàìîå ïîïóëÿðíîå ñëîâî
	map<string, size_t> freq; // êëþ÷ -- ñëîâî, çíà÷åíèå -- ÷àñòîòà ïîÿâëåíèÿ
	size_t i, maxFreq = 0; // ìàêñèìàëüíàÿ ÷àñòîòà ïîÿâëåíèÿ ñëîâà
	for (i = 0; i < book.size(); ++i) 
		if (!words2ignore.count(book[i]) && ++freq[book[i]] > maxFreq) {
			word = book[i];
			maxFreq = freq[word];
		}
}

int main() {
	map<string, size_t> freq; // êëþ÷ -- ñî÷åòàíèå ñëîâ, çíà÷åíèå -- ÷àñòîòà ïîÿâëåíèÿ
	map<string, size_t>::const_iterator iter;
	vector<pair<size_t, string>> res; // îòñîðòèðîâàííûé (ñì. äàëåå) ïî ÷àñòîòå êîíòåéíåð
	size_t i;
	clock_t begTime, endTime;
	ifstream ignoreFile("ignore.txt");
	while (ignoreFile >> word) words2ignore.insert(word);
	getBook();

		begTime = clock();
		getWord();
		for (i = 1; i < book.size() - 1; ++i) 
			if (book[i] == word) {
				++freq[book[i - 1] + " " + word];
				++freq[word + " " + book[i + 1]];
			}
		for (iter = freq.begin(); iter != freq.end(); ++iter) 
			res.push_back(make_pair(iter->second, iter->first));
		sort(res.rbegin(), res.rend());
		endTime = clock();
		N = min(N, res.size());
		for (i = 0; i < N; ++i) 
			cout << res[i].first << ' ' << res[i].second << '\n';
		cout << "time diff: " << double(endTime - begTime) / CLOCKS_PER_SEC << '\n'
		     << "words count: " << book.size() << "\n\n";

	return 0;
}
