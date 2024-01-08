import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Button, Card, Result, Table } from "antd";
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import Link from "next/link";
import { useContext, useEffect, useState } from "react";
import AppContext from "../../../shared/contexts/AppContext";
import { deleteAPI, get } from "../../../shared/utils/apiUtils";
import { CATEGORIES, POST_STATUS } from "../../../shared/constants/app-const";
import * as moment from 'moment'
import { deleteConfirm, successAlert } from "../../../shared/utils/alertUtils";
import styles from './PostManagement.module.scss';

const PostManagement = () => {
  const [data, setData] = useState([]);

  const columns = [
    {
      title: 'Tiêu đề (Click vào tiêu đề để xem)',
      dataIndex: 'title',
      key: 'title',
      render: (title, item) => {
        if (item.status == POST_STATUS.Active.id) {
          return <Link href={`/article?id=${item.slug}`} target="_blank" className={styles.preview}>{title}</Link>
        }

        return <Link href={`/admin/post-management/preview?id=${item.slug}`} target="_blank" className={styles.preview}>{title}</Link>
      }
    },
    {
      title: 'Người đăng',
      key: 'author',
      render: (_, item) => {
        const name = `${item.userFirstName || ''} ${item.userLastName || ''}`.trim();
        return name || item.userEmail;
      }
    },
    {
      title: 'Trạng thái',
      dataIndex: 'status',
      key: 'status',
      render: (value, item) => {
        const statusObj = Object.values(POST_STATUS).find(_ => _.id == value) || {};
        return <span className={`badge ${value == POST_STATUS.Active.id ? 'bg-info' : 'bg-warning'} text-dark me-1`}>
          {statusObj.name || ''}</span>;
      }
    },
    {
      title: 'Categories',
      key: 'categories',
      render: (value, item) => {
        return (item.categories || []).map(c => (<span className={`badge ${c.id == CATEGORIES.VietNam.id ? 'bg-info' : 'bg-warning'} text-dark me-1`}>{c.name}</span>))
      }
    },
    {
      title: 'Nhãn',
      dataIndex: 'tags',
      key: 'tags',
      render: (tags, item) => {
        return (tags || []).map(c => (<span className={`badge bg-info text-dark me-1`}>{c.text}</span>));
      }
    },
    {
      title: 'Ngày tạo',
      dataIndex: 'createdDate',
      key: 'createdDate',
      render: (value, item) => {
        return value ? moment(value).format('DD/MM/YYYY') : '';
      }
    },
    {
      title: 'Lên lịch',
      dataIndex: 'scheduleDate',
      key: 'scheduleDate',
      render: (value, item) => {
        return value ? moment(value).format('DD/MM/YYYY') : '';
      }
    },
    {
      title: 'Lượt xem',
      dataIndex: 'views',
      key: 'views',
      render: (_, item) => {
        return _ || 0;
      }
    },
    {
      title: '',
      key: 'actions',
      render: (_, item) => {
        return <>
          <Link key={`lnkUpdate${item.id}`} href={`/admin/post-management/new-post?id=${item.id}`}>
            <Button key={`btnUpdate${item.id}`} type="primary">Sửa</Button> &nbsp;
          </Link>
          <Button key={`btnDelete${item.id}`} type="primary" danger className="mt-1"
            onClick={() => onDeleteClicked(item.id)}>Xóa</Button>
        </>
      }
    },
  ];

  const { setLoading } = useContext(AppContext); 
  useEffect(() => {
    getData();
  }, []);

  const getData = () => {
    setLoading(true);
    get('/admin/post', false)
      .then(res => {
        if (res && res.data) {
          setData(res.data);
        }
      }).finally(() => setLoading(false));
  }

  const onDeleteClicked = (id) => {
    deleteConfirm('Xóa bài viết', 'Bạn có chắc?', result => {
      result && onDeletePost(id);
    });
  }

  const onDeletePost = (id) => {
    setLoading(true);

    deleteAPI(`/admin/post/${id}`, false)
      .then(res => {
        if (res && res.data) {
          const index = data.findIndex(_ => _.id == id);
          if (index > -1) {
            data.splice(index, 1);
            setData([...data]);
          }

          successAlert('Xóa thành công');
        }
      }).finally(() => setLoading(false));
  }

  return <>
    <Card>
      <h2>Quản lý bài viết</h2>
      <br />

      <div className="d-flex">
        <Link href="/admin/post-management/new-post">
          <Button type="primary" className="ms-auto">
            <><FontAwesomeIcon icon={faPlus} />&nbsp; Bài viết mới</>
          </Button>
        </Link>
      </div>

      <Table dataSource={data} columns={columns} className="mt-2" />
    </Card>
  </>;
}

export default PostManagement;