import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Button, Card, Result, Table } from "antd";
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import Link from "next/link";
import { useContext, useEffect, useRef, useState } from "react";
import AppContext from "../../../shared/contexts/AppContext";
import { deleteAPI, get } from "../../../shared/utils/apiUtils";
import { CATEGORIES, POST_STATUS } from "../../../shared/constants/app-const";
import * as moment from 'moment'
import { deleteConfirm, successAlert } from "../../../shared/utils/alertUtils";
import UserModal from "./UserModal";

const UserManagement = () => {
  const userModalRef = useRef();
  const [data, setData] = useState([]);

  const columns = [
    {
      title: 'Username',
      dataIndex: 'email',
      key: 'email',
    },
    {
      title: 'Tên',
      key: 'name',
      render: (_, item) => {
        const name = `${item.firstName || ''} ${item.lastName || ''}`.trim();
        return name || item.email;
      }
    },
    {
      title: 'Chức vụ',
      dataIndex: 'role',
      key: 'role',
      render: (value, item) => {
        const statusObj = Object.values(POST_STATUS).find(_ => _.id == value) || {};
        return statusObj.name || '';
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
      title: 'Ngày tạo',
      dataIndex: 'createdDate',
      key: 'createdDate',
      render: (value, item) => {
        return value ? moment(value).format('DD/MM/YYYY') : '';
      }
    },
    {
      title: '',
      key: 'actions',
      render: (_, item) => {
        return <>
          <Button key={`btnUpdate${item.id}`} type="primary" size="small">Sửa</Button> &nbsp;
          <Button key={`btnDelete${item.id}`} type="primary" danger className="mt-1"
            onClick={() => onDeleteClicked(item.id)} size="small">Xóa</Button>
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
    get('/admin/account', false)
      .then(res => {
        if (res && res.data) {
          setData(res.data);
        }
      }).finally(() => setLoading(false));
  }

  const onDeleteClicked = (id) => {
    deleteConfirm('Xóa người dùng', 'Ư', result => {
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

  const onUserUpdated = user => {
    const index = data.findIndex(_ => _.id == user.id);

    if (index > -1) {
      data[index] = user;
      setData([...data]);
    }
  }

  return <>
    <Card>
      <h2>Quản lý người dùng</h2>
      <br />

      <div className="d-flex">
        <Button type="primary" className="ms-auto" onClick={() => userModalRef.current.showModal()}>
          <><FontAwesomeIcon icon={faPlus} />&nbsp; Tạo người dùng</>
        </Button>
      </div>

      <Table dataSource={data} columns={columns} className="mt-2" />
      <UserModal ref={userModalRef} onCreated={user => setData([user, ...data])}
        onUpdated={onUserUpdated}/>
    </Card>
  </>;
}

export default UserManagement;