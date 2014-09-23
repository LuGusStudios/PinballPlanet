using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ILugusAudio
{
	LugusAudioChannel GetChannel(Lugus.AudioChannelType type);
	
	// float GlobalVolume{ get; set; } // change AudioListener.volume, also store in LugusConfig for good measure
	
	LugusAudioChannel Music();
	LugusAudioChannel Ambient();
	LugusAudioChannel SFX();
	LugusAudioChannel Speech();
	
	LugusAudioChannel Custom1();
	LugusAudioChannel Custom2();
	LugusAudioChannel Custom3();
}

public class LugusAudioDefault : MonoBehaviour, ILugusAudio
{
	protected List<LugusAudioChannel> _channels = new List<LugusAudioChannel>();
	public List<LugusAudioChannel> Channels
	{
		get{ return _channels; }
	}
	
	public LugusAudioChannel GetChannel(Lugus.AudioChannelType type)
	{
		LugusAudioChannel output = null;
		
		foreach( LugusAudioChannel channel in _channels )
		{
			if( channel.ChannelType == type )
			{
				output = channel;
				break;
			}
		}
		
		if( output == null )
		{
			output = CreateChannel( type );
		}
		
		return output;
	}
	
	protected LugusAudioChannel CreateChannel(Lugus.AudioChannelType type)
	{
		LugusAudioChannel channel = new LugusAudioChannel( type );
		
		_channels.Add( channel );
		
		return channel;
	}
	
	// Default channels below. Will probably suffice for most projects.
	protected LugusAudioChannel music = null;
	protected LugusAudioChannel ambient = null;
	protected LugusAudioChannel sfx = null;
	protected LugusAudioChannel speech = null;
	protected LugusAudioChannel custom1 = null;
	protected LugusAudioChannel custom2 = null;
	protected LugusAudioChannel custom3 = null;

	public LugusAudioChannel Music()
	{ 
		if (music == null)
			music = GetChannel(Lugus.AudioChannelType.BackgroundMusic); 

		return music;
	}

	public LugusAudioChannel Ambient()
	{ 
		if (ambient == null)
			ambient =  GetChannel(Lugus.AudioChannelType.BackgroundAmbient); 

		return ambient;
	}

	public LugusAudioChannel SFX ()
	{
		if (sfx == null)
			sfx = GetChannel(Lugus.AudioChannelType.ForegroundSFX);
			
		return sfx;
	}

	public LugusAudioChannel Speech()
	{
		if (speech == null)
			speech = GetChannel (Lugus.AudioChannelType.ForegroundSpeech);
			
		return speech;
	}
	
	public LugusAudioChannel Custom1()
	{
		if (custom1 == null)
			custom1 = GetChannel (Lugus.AudioChannelType.Custom1);

		return custom1;
	}

	public LugusAudioChannel Custom2()
	{
		if (custom2 == null)
			custom2 = GetChannel (Lugus.AudioChannelType.Custom2);
		
		return custom2;
	}

	public LugusAudioChannel Custom3()
	{
		if (custom3 == null)
			custom3 = GetChannel (Lugus.AudioChannelType.Custom3);
		
		return custom3;
	}
}