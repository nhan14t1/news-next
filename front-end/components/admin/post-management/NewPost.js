import { Button, Empty, Input, Select, Typography } from 'antd';
import React, { useContext, useEffect, useState } from 'react';
import { CATEGORIES, IMAGE_POST_PREFIX, POST_STATUS } from '../../../shared/constants/app-const';
import { toSlug } from '../../../shared/utils/stringUtils';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faRedoAlt } from '@fortawesome/free-solid-svg-icons';
import { get, post, put } from '../../../shared/utils/apiUtils';
import { useRouter } from 'next/router';
import HtmlEditor from '../../shared/HtmlEditor';
import ThumbnailUpload from './components/ThumbnailUpload';
import AppContext from '../../../shared/contexts/AppContext';
import { useSearchParams } from 'next/navigation';
import { compressBase64 } from '../../../shared/utils/imageCompressUtils';

const NewPost = props => {
  const router = useRouter();
  const [postObj, setPostObj] = useState({});
  const [isError, setIsError] = useState(false);
  const { setLoading } = useContext(AppContext);

  const categoryOptions = Object.values(CATEGORIES).map(_ => ({ value: _.id, label: _.name }));

  const isEdit = () => postObj && postObj.id;
  const params = useSearchParams()

  useEffect(() => {
    const id = params.get('id');
    id && getData(id);
  }, []);

  const getData = (id) => {
    setLoading(true)
    get(`/admin/post/${id}`)
      .then(res => {
        if (res && res.data) {
          const postObj = res.data;
          postObj.categoryIds = (postObj.categories || []).map(_ => _.id);
          setPostObj(postObj);
        }
      }).catch(err => {
        setIsError(true);
      }).finally(() => setLoading(false));
  }

  const onTitleBlur = () => {
    if (!postObj.slug && postObj.title && postObj.title.trim()) {
      setPostObj({ ...postObj, slug: toSlug(postObj.title) });
    }
  }

  const onCreate = (postStatus = POST_STATUS.Active.id) => {
    if (!validateCreation()) {
      return;
    }

    var imageUrls = processImageUrls();

    const request = { ...postObj, imageUrls };
    request.status = postStatus;

    setLoading(true);
    post('/admin/post', request, false)
      .then(res => {
        if (res) {
          router.push('/admin/post-management');
        }
      }).finally(() => {
        setLoading(false);
      });
  }

  const onUpdate = (postStatus = POST_STATUS.Active.id) => {
    if (!validateCreation()) {
      return;
    }

    var imageUrls = processImageUrls();

    const request = { ...postObj, imageUrls };
    request.status = postStatus;

    setLoading(true);
    put('/admin/post', request, false)
      .then(res => {
        router.push('/admin/post-management');
      }).finally(() => {
        setLoading(false);
      });
  }

  // In case we insert an image, then we delete the image on UI => Need to remove this image to save server storage.
  const processImageUrls = () => {
    // To make sure the regex won't detect 2 result into 1 result
    const content = (postObj.content || '').replaceAll(`<img`, `\n<img`);

    const pattern = new RegExp(`${IMAGE_POST_PREFIX}.*\.(jpg|png|jpeg|bmp|svg|webp)`, 'g');
    return content.match(pattern);
  }

  const validateCreation = () => {
    return true;
  }

  const onThumbnailChanged = async (base64) => {
    const compressedBase64 = await compressBase64(base64, 0.3, 500);
    setPostObj({ ...postObj, thumbnail: { ...postObj.thumbnail, base64: compressedBase64 } });
  }

  if (isError) {
    return <Empty description="Không có dữ liệu" className='pt-5'></Empty>
  }

  return <div className='new-post'>
    <div className='app-box'>
      <h2>{isEdit() ? 'Chỉnh sửa bài viết' : 'Tạo bài viết'}</h2>
      <b className='mt-5 d-block'>Tiêu đề:</b>
      <div className='mt-2'>
        <Input key='title' className='w-100' placeholder='Nhập tiêu đề...'
          value={postObj.title}
          onChange={e => setPostObj({ ...postObj, title: e.target.value })}
          onBlur={e => onTitleBlur()}></Input>
      </div>

      <div className='mt-3'>
        <b>Slug</b>&nbsp;&nbsp;
        <Button key="btnSlug" type='primary' size='small' ghost
          disabled={!postObj.title || !postObj.title.trim()}
          onClick={() => setPostObj({ ...postObj, slug: toSlug(postObj.title) })}>
          <FontAwesomeIcon icon={faRedoAlt} />&nbsp;
          Tạo slug
        </Button>
      </div>
      <div className='mt-2'>
        <Input key='slug' className='w-100' placeholder='Ví dụ: an-gi-hom-nay'
          value={postObj.slug}
          onChange={e => setPostObj({ ...postObj, slug: e.target.value })}></Input>
      </div>

      <b className='mt-3 d-block'>Tóm tắt:</b>
      <div className='mt-2'>
        <Input key='introText' className='w-100' placeholder='Giới thiệu vắn tắt để gây sự tò mò'
          value={postObj.introText}
          onChange={e => setPostObj({ ...postObj, introText: e.target.value })}></Input>
      </div>

      <b className='mt-3 d-block'>Categories:</b>
      <div className='mt-2'>
        <Select
          key='categories'
          mode="multiple"
          allowClear
          style={{
            width: '100%',
          }}
          placeholder="Chọn 1 hoặc nhiều category"
          value={postObj.categoryIds || []}
          onChange={(e) => setPostObj({ ...postObj, categoryIds: e })}
          options={categoryOptions}
        />
      </div>

      <b className='mt-3 d-block'>Nhãn (Tùy chọn):</b>
      <div className='mt-2'>
        <Input key='label' className='w-100' placeholder='Chưa có chức năng nhãn'></Input>
      </div>

      <div className='mt-3'>
        <b>Thumbnail:</b>

        {isEdit() &&
          <span> &nbsp;<Typography.Text type="secondary">(Chế độ edit sẽ không hiển thị thumbnail trước đó, nhưng vẫn có thể đổi thumbnail)</Typography.Text></span>
        }
      </div>
      <div className='mt-2'>
        <ThumbnailUpload image={postObj.thumbnail} onChange={thumbnail => setPostObj({ ...postObj, thumbnail })}
          onCropped={base64 => onThumbnailChanged(base64)} onCancelled={() => setPostObj({ ...postObj, thumbnail: null })} />
      </div>

      <div className='mt-3'><b>Nội dung:</b></div>
      <div className='mt-2'>
        <HtmlEditor key='html-editor' value={postObj.content} onChange={content => setPostObj({ ...postObj, content })} />
      </div>

      <div className='mt-5 pt-5 pt-sm-3 d-flex'>
        <div className='ms-auto'>
          {!isEdit() &&
            <>
              <Button key='btn-draft' type='default' onClick={() => onCreate(POST_STATUS.Draft.id)}>Lưu nháp</Button>
              <Button key='btn-publish' className='ms-2' type='primary'
                onClick={() => onCreate()}>Xuất bản</Button>
            </>
          }

          {isEdit() &&
            <>
              {postObj.status != POST_STATUS.Active.id &&
                <Button key='btn-draft' className='ms-2' type='default'
                  onClick={() => onUpdate(POST_STATUS.Draft.id)}>Lưu nháp</Button>
              }

              <Button key='btn-publish' className='ms-2' type='primary'
                onClick={() => onUpdate()}>{postObj.status != POST_STATUS.Active.id ? 'Xuất bản' : 'Lưu'}</Button>
            </>
          }

          <Button key='btn-schedule' className='ms-2' type='primary' ghost onClick={() => alert('Chức năng này chưa có')}>Lên lịch</Button>
        </div>
      </div>
    </div>
  </div>
}

export default NewPost;