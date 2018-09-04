//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.10
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace Dcp.Net.MQ.RocketMQ.SDK {

public class PullResultONS : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal PullResultONS(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(PullResultONS obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~PullResultONS() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          ONSClient4CPPPINVOKE.delete_PullResultONS(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public PullResultONS(ONSPullStatus status) : this(ONSClient4CPPPINVOKE.new_PullResultONS__SWIG_0((int)status), true) {
  }

  public PullResultONS(ONSPullStatus pullStatus, long nextBeginOffset, long minOffset, long maxOffset) : this(ONSClient4CPPPINVOKE.new_PullResultONS__SWIG_1((int)pullStatus, nextBeginOffset, minOffset, maxOffset), true) {
  }

  public ONSPullStatus pullStatus {
    set {
      ONSClient4CPPPINVOKE.PullResultONS_pullStatus_set(swigCPtr, (int)value);
    } 
    get {
      ONSPullStatus ret = (ONSPullStatus)ONSClient4CPPPINVOKE.PullResultONS_pullStatus_get(swigCPtr);
      return ret;
    } 
  }

  public long nextBeginOffset {
    set {
      ONSClient4CPPPINVOKE.PullResultONS_nextBeginOffset_set(swigCPtr, value);
    } 
    get {
      long ret = ONSClient4CPPPINVOKE.PullResultONS_nextBeginOffset_get(swigCPtr);
      return ret;
    } 
  }

  public long minOffset {
    set {
      ONSClient4CPPPINVOKE.PullResultONS_minOffset_set(swigCPtr, value);
    } 
    get {
      long ret = ONSClient4CPPPINVOKE.PullResultONS_minOffset_get(swigCPtr);
      return ret;
    } 
  }

  public long maxOffset {
    set {
      ONSClient4CPPPINVOKE.PullResultONS_maxOffset_set(swigCPtr, value);
    } 
    get {
      long ret = ONSClient4CPPPINVOKE.PullResultONS_maxOffset_get(swigCPtr);
      return ret;
    } 
  }

  public SWIGTYPE_p_std__vectorT_ons__Message_t msgFoundList {
    set {
      ONSClient4CPPPINVOKE.PullResultONS_msgFoundList_set(swigCPtr, SWIGTYPE_p_std__vectorT_ons__Message_t.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = ONSClient4CPPPINVOKE.PullResultONS_msgFoundList_get(swigCPtr);
      SWIGTYPE_p_std__vectorT_ons__Message_t ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_std__vectorT_ons__Message_t(cPtr, false);
      return ret;
    } 
  }

}

}
