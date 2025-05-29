#pragma once

#ifdef WRAPPERAPP_EXPORTS
#define WRAPPERAPP_API __declspec(dllexport)
#else
#define WRAPPERAPP_API __declspec(dllimport)
#endif

extern "C" {
	WRAPPERAPP_API int AddItem(const char* name);
	WRAPPERAPP_API const char* GetItem(int id);
	WRAPPERAPP_API bool RemoveItem(int id);
	WRAPPERAPP_API int GetItemCount();
	WRAPPERAPP_API void UpdateItem(int id, const char* name);
	WRAPPERAPP_API const char* ListItems();
}