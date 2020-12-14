using System.Collections.Generic;
using UnityEngine;

namespace StaTech{

	public enum TextKey{
		Progress,
		ReferenceEmpty,
		PingSourceObject,
		ShowReference
	}

	public static class Define{

		public static string GetText(TextKey key){
			if(Application.systemLanguage == SystemLanguage.Japanese){
				return JP(key);
			}else{
				return EN(key);
			}
		}

		private static string JP(TextKey key){
			switch(key){
			case TextKey.Progress:
				return "参照を検索中...";
			case TextKey.ReferenceEmpty:
				return "このコンポーネントはどこからも参照されていません";
			case TextKey.PingSourceObject:
				return "検索元のオブジェクトを選択";
			case TextKey.ShowReference:
				return "参照を表示";
			}
			return "";
		}

		private static string EN(TextKey key){
			switch(key){
			case TextKey.Progress:
				return "Finding References Progress";
			case TextKey.ReferenceEmpty:
				return "This component has no references";
			case TextKey.PingSourceObject:
				return "Ping source object";
			case TextKey.ShowReference:
				return "Show references";
			}	
			return "";
		}
	}
}