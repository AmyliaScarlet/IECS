import * as jQuery from 'jquery'

interface IECS {
    Init();
    OnLoad();
    OnDocumentReady();
    OnAbort();
    OnBlur();
    OnClick();
    OnContextMenu();
} 

interface cIECS {
    cOnLoad();
    cOnDocumentReady();
    cOnFinish();

} 

class IECSSys implements IECS, cIECS {
    cOnLoad() {
        
    }
    cOnDocumentReady() {
        
    }
    cOnFinish() {
        
    }


    Init() {
        
    }
    OnLoad() {
        
    }
    OnDocumentReady() {
        
    }
    OnAbort() {
        
    }
    OnBlur() {
        
    }
    OnClick() {
        
    }
    OnContextMenu() {
        
    }
}