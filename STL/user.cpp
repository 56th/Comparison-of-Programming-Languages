#include <iostream>
#include <fstream>
#include <vector>
#include <map>
#include <set>
#include <string>
#include <algorithm>
#include <ctime> // замерить время
#include <cctype> // isalnum

using namespace std; // программа коротенькая, так что можно и расслабиться

// сегмент данных

vector<string> book; // текст книги 
string word; // самое популярное слово
set<string> words2ignore; // игнорируемые слова (предлоги, артикли и т.д.)
unsigned N = 15; // количество выводимых сочетаний слов


void getBook() { // считываем текст книжки из стандартного ввода в контейнер
	while (cin >> word) {
		transform(word.begin(), word.end(), word.begin(), ::tolower); // прописные буквы к строчным
		word.erase(remove_if(word.begin(), word.end(), [](char c){return !isalnum(c);}), word.end()); // избавляемся от лишних символов в слове
		if (word.length()) book.push_back(word);
	}
}

void getWord() { // находим самое популярное слово
	map<string, size_t> freq; // ключ -- слово, значение -- частота появления
	size_t i, maxFreq = 0; // максимальная частота появления слова
	for (i = 0; i < book.size(); ++i) 
		if (!words2ignore.count(book[i]) && ++freq[book[i]] > maxFreq) {
			word = book[i];
			maxFreq = freq[word];
		}
}

int main() {
	map<string, size_t> freq; // ключ -- сочетание слов, значение -- частота появления
	map<string, size_t>::const_iterator iter;
	vector<pair<size_t, string>> res; // отсортированный (см. далее) по частоте контейнер
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
