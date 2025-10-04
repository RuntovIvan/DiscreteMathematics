#pragma once
#include <iostream>
#include <vector>
using namespace std;

class Set
{
	vector<int> elements;

public:
	static Set GetUniversum();

	void InsertElement(int el);
	void RemoveElement(int el);
	bool Contains(int el);
	int GetLength();

	Set Union(const Set& secondSet) const;
	Set Intersection(const Set& secondSet) const;
	Set Difference(const Set& secondSet) const;
	Set SymmetricDifference(const Set& secondSet) const;
	Set Complement() const;

	void Clear();
	Set& operator=(const Set& secondSet);
	friend ostream& operator<<(ostream& out, const Set& set);
};

Set Set::GetUniversum()
{
	Set universum;
	for (int i = -50; i <= 50; i++)
		universum.InsertElement(i);
	return universum;
}

void Set::InsertElement(int el)
{
	auto it = find(elements.begin(), elements.end(), el);
	if (it == elements.end())
		elements.push_back(el);
}

void Set::RemoveElement(int el)
{
	auto it = find(elements.begin(), elements.end(), el);
	if (it != elements.end())
		elements.erase(it);
}

bool Set::Contains(int el)
{
	auto it = find(elements.begin(), elements.end(), el);
	return it != elements.end();
}

int Set::GetLength()
{
	return elements.size();
}

Set Set::Union(const Set& secondSet) const
{
	Set result;
	for (int i = 0; i < this->elements.size(); i++)
		result.InsertElement(this->elements[i]);

	for (int i = 0; i < secondSet.elements.size(); i++)
		result.InsertElement(secondSet.elements[i]);

	return result;
}

Set Set::Intersection(const Set& secondSet) const
{
	Set result;
	for (int i = 0; i < this->elements.size(); i++)
	{
		auto it = find(secondSet.elements.begin(), secondSet.elements.end(), this->elements[i]);
		if (it != secondSet.elements.end()) // если есть во 2-м
			result.elements.push_back(this->elements[i]);
	}

	return result;
}

Set Set::Difference(const Set& secondSet) const
{
	Set result;
	for (int i = 0; i < this->elements.size(); i++)
	{
		auto it = find(secondSet.elements.begin(), secondSet.elements.end(), this->elements[i]);
		if (it == secondSet.elements.end()) // если нет во 2-м
			result.elements.push_back(this->elements[i]);
	}

	return result;
}

Set Set::SymmetricDifference(const Set& secondSet) const
{
	Set set1 = this->Difference(secondSet);
	Set set2 = secondSet.Difference(*this);
	Set result = set1.Union(set2);

	return result;
}

Set Set::Complement() const
{
	Set universum = GetUniversum();
	Set result = universum.Difference(*this);
	return result;
}

Set& Set::operator=(const Set& secondSet)
{
	this->Clear();
	for (int i = 0; i < secondSet.elements.size(); i++)
		this->elements.push_back(secondSet.elements[i]);
	return *this;
}

void Set::Clear()
{
	while (this->elements.size() != 0)
		this->elements.pop_back();
}

ostream& operator<<(ostream& out, const Set& set)
{
	if (set.elements.size() != 0)
	{
		cout << "{";
		for (int i = 0; i < set.elements.size(); i++)
			cout << set.elements[i] << (i == set.elements.size() - 1 ? "}" : ", ");
	}
	else
		cout << "#";

	return out;
}