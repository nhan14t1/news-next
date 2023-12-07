import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Button, Card, Table } from "antd";
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import Link from "next/link";
import { useContext, useEffect, useState } from "react";
import AppContext from "../../../shared/contexts/AppContext";
import { get } from "../../../shared/utils/apiUtils";
import { CATEGORIES, POST_STATUS } from "../../../shared/constants/app-const";
import * as moment from 'moment'

const PostManagement = () => {
  const [data, setData] = useState([]);
  const dataSource = [
    {
      id: '1',
      author: 'Mike',
      views: 32,
      title: '10 Downing Street',
    },
    {
      id: '2',
      author: 'John',
      views: 42,
      title: '10 Downing Street 2',
    },
  ];

  const columns = [
    {
      title: 'Tiêu đề',
      dataIndex: 'title',
      key: 'title',
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
        return statusObj.name || '';
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
      title: 'Tags',
      dataIndex: 'tags',
      key: 'tags',
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
          <Button key={`btnUpdate${item.id}`} type="primary">Sửa</Button> &nbsp;
          <Button key={`btnDelete${item.id}`} type="primary" danger>Xóa</Button>
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
    get('/admin/post')
      .then(res => {
        if (res && res.data) {
          setData(res.data);
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

      <Table dataSource={data} columns={columns} className="mt-2" />;
    </Card>
  </>;
}

export default PostManagement;