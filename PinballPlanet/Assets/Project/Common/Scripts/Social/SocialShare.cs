using UnityEngine;
using System.Collections;

public class SocialShare : LugusSingletonRuntime<SocialShareBasic> 
{
			
}

public class SocialShareBasic : LugusSingletonRuntime<SocialShareBasic>
{
	public FacebookBasic facebook = null;
	public TwitterBasic twitter = null;

	public override void InitializeSingleton ()
	{
		base.InitializeSingleton ();
		facebook = FacebookBasic.use;
		twitter = TwitterBasic.use;
	}

	public void InitializeSocial()
	{
		facebook.Initialize();
	}
}