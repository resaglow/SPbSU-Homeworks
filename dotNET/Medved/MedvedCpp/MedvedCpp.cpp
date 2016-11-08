// MedvedCpp2.cpp : main project file.

#include "stdafx.h"

using namespace System;

private ref class MedvedCPlusPlus sealed : MedvedFS::MedvedFSharp
{
public:
	void MeetMedved() override
	{
		System::Console::WriteLine("Preved from C++");
		MedvedFS::MedvedFSharp::MeetMedved();
	}
};

int main()
{
	MedvedCPlusPlus^ medved = gcnew MedvedCPlusPlus();
	medved->MeetMedved();
	return 0;
}
