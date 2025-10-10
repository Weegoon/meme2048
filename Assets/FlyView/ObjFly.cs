//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ObjFly : MonoBehaviour
//{
//	private Vector2 Position {
//		get {
//			return new Vector2 (transform.position.x, transform.position.y);
//		}
//		set {
//			transform.position = new Vector3 (value.x, value.y, transform.position.z);
//		}
//	}

//	Coroutine coFly;

//	public void FlyTo (Vector3 position, float duration, Callback callBack)
//	{
//		StartCoroutineCoFly (position, duration, callBack);
//	}

//	public void FlyTo (Transform target, float duration, Callback callBack, int index)
//	{
//		StartCoroutineCoFlyTransform (target, duration, callBack, index);
//	}

//	public void FlySlowDownTo (Vector3 position, float duration)
//	{
//		FlyTo (position, duration, delegate {
//		});
//	}

//	void StartCoroutineCoFly (Vector3 position, float duration, Callback callBack)
//	{
//		try {
//			StopCoroutine (coFly);
//		} catch {
//		}
//		coFly = StartCoroutine (IEFly (position, duration, callBack));
//	}

//	void StartCoroutineCoFlyTransform (Transform target, float duration, Callback callBack, int index)
//	{
//		try {
//			StopCoroutine (coFly);
//		} catch {
//		}
//		coFly = StartCoroutine (IEFlyTransform (target, duration, callBack, index));
//	}

//	IEnumerator IEFly (Vector3 position, float duration, Callback callBack)
//	{
//		float t = 0;
//		Vector2 p = Position;
//		while (t < duration) {
//			t += Time.deltaTime;
//			Position = Vector2.Lerp (p, position, GetX (t / duration));
//			yield return null;
//		}
//		Position = position;
//		callBack ();
//	}

//	IEnumerator IEFlyTransform (Transform target, float duration, Callback callBack, int index)
//	{   
//        float t = 0;
//        Vector2 origin = Position;
//        float xt = index % 2 == 0 ? (target.position.x + Position.x) * 1.25f : -(target.position.x + Position.x) * 1.25f;
//        float yt = 2f * (origin.y + target.position.y) / 3f;
//        Vector2 tVec = new Vector2(xt, yt);
//        while (t < duration)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
//        {
//            t += Time.deltaTime;
//            Position = QuadraticCurveLerp(origin, tVec, target.position, GetX(t / duration));
//            yield return null;
//        }
//        Position = target.position;
//        callBack();
//    }

//    float GetX (float t)
//	{
//		t -= 1;
//		return  t * t * t + 1;
//	}  
    
//    Vector2 QuadraticCurveLerp(Vector2 a, Vector2 b, Vector2 c, float t)
//    {
//        Vector2 p0 = Lefp(a, b, t);
//        Vector2 p1 = Lefp(b, c, t);
//        return Lefp(p0, p1, t);
//    }

//    Vector2 Lefp(Vector2 a, Vector2 b, float t)
//    {
//        return a + (b - a) * t;
//    }
//}
