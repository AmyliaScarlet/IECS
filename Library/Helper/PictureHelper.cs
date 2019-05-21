using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace IECS.Library
{
    /// <summary>
    /// 图片处理类
    /// </summary>
    public class PictureHelper
    {
        #region 图片缩放

        /// <summary>
        /// 图片缩放
        /// </summary>
        /// <param name="strOldPic">源图文件名(包括路径)</param>
        /// <param name="strNewPic">缩小后保存为文件名(包括路径)</param>
        /// <param name="intWidth">缩小至宽度</param>
        /// <param name="intHeight">缩小至高度</param>
        public void ZoomPic(string strOldPic, string strNewPic, int intWidth, int intHeight)
        {

            Bitmap objPic, objNewPic;
            try
            {
                objPic = new Bitmap(strOldPic);
                objNewPic = new Bitmap(objPic, intWidth, intHeight);
                objNewPic.Save(strNewPic);

            }
            catch (Exception exp) { throw exp; }
            finally
            {
                objPic = null;
                objNewPic = null;
            }
        }

        /// <summary>
        /// 按比例缩小图片，自动计算高度
        /// </summary>
        /// <param name="strOldPic">源图文件名(包括路径)</param>
        /// <param name="strNewPic">缩小后保存为文件名(包括路径)</param>
        /// <param name="intWidth">缩小至宽度</param>
        public void ZoomPicAutoHeight(string strOldPic, string strNewPic, int intWidth)
        {

            Bitmap objPic, objNewPic;
            try
            {
                objPic = new Bitmap(strOldPic);
                int intHeight = Convert.ToInt32(((intWidth * 1.0) / (objPic.Width * 1.0)) * objPic.Height);
                objNewPic = new Bitmap(objPic, intWidth, intHeight);
                objNewPic.Save(strNewPic, objPic.RawFormat);

            }
            catch (Exception exp) { throw exp; }
            finally
            {
                objPic = null;
                objNewPic = null;
            }
        }

        /// <summary>
        /// 按比例缩小图片，自动计算宽度
        /// </summary>
        /// <param name="strOldPic">源图文件名(包括路径)</param>
        /// <param name="strNewPic">缩小后保存为文件名(包括路径)</param>
        /// <param name="intHeight">缩小至高度</param>
        public void ZoomPicAutoWidth(string strOldPic, string strNewPic, int intHeight)
        {

            Bitmap objPic, objNewPic;
            try
            {
                objPic = new Bitmap(strOldPic);
                int intWidth = Convert.ToInt32(((intHeight * 1.0) / objPic.Height) * objPic.Width);
                objNewPic = new Bitmap(objPic, intWidth, intHeight);
                objNewPic.Save(strNewPic, objPic.RawFormat);

            }
            catch (Exception exp) { throw exp; }
            finally
            {
                objPic = null;
                objNewPic = null;
            }
        }

        #endregion
    }
}
